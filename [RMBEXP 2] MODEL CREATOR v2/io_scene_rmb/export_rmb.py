import bpy
import bmesh
import os
from .binaresLib import BinaryReader
from .functionsLib import MeshType, check_mesh_type, split_mesh_by_faces, convert_quads_to_triangles


class MeshInfo():
    def __init__(self):
        self.name = ''
        self.parentBone = ''
        self.hasRig = False
        self.textureIndex = 0
        self.boneMapCount = 0
        self.verticesCount = 0
        self.indicesCount = 0
        self.vertices = []
        self.triangles = []
        self.uv = []
        self.UVFLIP = True


class ExportRMB():
    def __init__(self, operator):
        self.operator = operator

    def log(self, message, type='INFO'):
        self.operator.report({type}, message)

    def get_textures_from_objects(self, objects):
        textures = []
        for obj in objects:
            if obj.type != 'MESH':
                continue
            if len(obj.material_slots) > 0:
                for slot in obj.material_slots:
                    if slot.material and slot.material.use_nodes:
                        for node in slot.material.node_tree.nodes:
                            if node.type == 'TEX_IMAGE':
                                if node.image not in textures:
                                    textures.append(node.image)
            else:
                self.log(f'Object {obj.name} has no materials', 'WARNING')

        return textures

    def get_texture_index(self, mesh, textures):
        obj = bpy.data.objects.get(mesh.name)
        if obj:
            for slot in obj.material_slots:
                material = slot.material
                if material and material.use_nodes:
                    for node in material.node_tree.nodes:
                        if node.type == 'TEX_IMAGE':
                            if node.image in textures:
                                return textures.index(node.image)
                            
        self.log(f'Texture not found for mesh {mesh.name}', 'WARNING')
        return 0

    def get_mesh_indices(self, mesh):
        indices = []

        for face in mesh.polygons:
            indices.append(face.vertices)

        return indices

    def convert_mesh_faces_to_triangles(self, mesh):
        triangles = []

        for poly in mesh.polygons:
            if len(poly.vertices) == 3:
                triangles.append(poly.vertices)
            else:
                # split the polygon into triangles
                print(f'WARNING: Polygon with {len(poly.vertices)} vertices')
                for i in range(1, len(poly.vertices) - 1):
                    triangles.append((poly.vertices[0], poly.vertices[i], poly.vertices[i+1]))

        return triangles

    def get_mesh_uv_coordinates(self, mesh):
        if not mesh.uv_layers:
            return []

        uv_coordinates = [0] * len(mesh.vertices)
        uv_layer = mesh.uv_layers.active
        print(f'UV Lauyer: {len(uv_layer.name)}')
        print(f'Loops: {len(mesh.loops)}')

        for face in mesh.polygons:
            for loop_index in face.loop_indices:
                vertex_index = mesh.loops[loop_index].vertex_index
                uv_coordinates[vertex_index] = uv_layer.data[loop_index].uv

        return uv_coordinates

    def remove_suffix(self, s):
        return s.rsplit('.', 1)[0]

    def parse(self, filename: str, reader: BinaryReader):
        # set mode to - object
        bpy.ops.object.mode_set(mode='OBJECT')
        ret = {'FINISHED'}
        # get selected objects or all objects
        objects = bpy.context.selected_objects if len(bpy.context.selected_objects) > 0 else bpy.data.objects
        if not objects:
            self.log('No object selected for export', 'ERROR')
            ret = {'CANCELLED'}
            return ret

        # get textures count of Blender Object
        textures = self.get_textures_from_objects(objects)
        # clean textures list from specular and normal textures
        if len(textures) > 0:
            textures = [tex for tex in textures if not tex.name.endswith('_sp.dds') and not tex.name.endswith('_n.dds')]

        # get meshes count of Blender Object
        meshes = []
        for obj in objects:
            if obj.type == 'MESH':
                meshes.append(obj.data)

        # header
        reader.write_unknown(count=20)               # 20 magic unknown bytes
        reader.write_int32((len(textures)))          # texture_count
        reader.write_int32((len(meshes)))            # mesh_count
        reader.write_int32((0))                      # bone_count
        reader.write_int32((0))                      # texture_offset ?

        # write textures names
        print(f'Texture count: {len(textures)}')
        for texture in textures:
            # 260 for each texture name
            tex_name = texture.name
            # remove blender internal index suffix if exists
            if not tex_name.endswith('.dds'):
                tex_name = self.remove_suffix(texture.name)

            print(f'\tAdd texture: {tex_name}')
            off = 260 - len(tex_name)
            reader.write_string(tex_name)
            reader.write_unknown(count=off)

        # 2156 for each mesh
        # write mesh info
        meshes_info = []
        print(f'Mesh info count: {1}, start offset: {reader.tell()}')
        for mesh in meshes:
            mesh_name = self.remove_suffix(mesh.name)

            # check mesh has uv coordinates
            uv = self.get_mesh_uv_coordinates(mesh)
            if len(uv) == 0:
                # fill uv with 0, 0
                uv = [(0, 0)] * len(mesh.vertices)
                self.log(f'Mesh {mesh_name} has no UV coordinates', 'WARNING')
                # ret = {'CANCELLED'}
                # return ret

            m = MeshInfo()
            m.name = mesh_name
            m.parentBone = 'root'
            m.textureIndex = self.get_texture_index(mesh, textures)
            m.vertices = mesh.vertices
            m.triangles = self.convert_mesh_faces_to_triangles(mesh)
            m.uv = uv
            m.verticesCount = len(m.vertices)
            m.indicesCount = len(m.triangles) * 3

            print(f'\tAdd mesh: {m.name}')

            # 8 for mesh name offset
            print(f'T1: {reader.tell()}')
            reader.write_unknown(count=8)
            
            # 64 for mesh name
            name_off = 64 - len(m.name)
            reader.write_string(m.name)
            reader.write_unknown(count=name_off)

            # 64 for parent bone name
            parent_bone_off = 64 - len(m.parentBone)
            reader.write_string(m.parentBone)
            reader.write_unknown(count=parent_bone_off)

            # 20 for 5 * 4 mesh info values
            # write mesh info 5 values
            # 0, 0, 0, 546, 1164
            reader.write_int32(1 if m.hasRig else 0)
            reader.write_int32(m.textureIndex)
            reader.write_int32(m.boneMapCount)
            reader.write_int32(m.verticesCount)
            reader.write_int32(m.indicesCount)

            # 2156 - (8 + 64 + 64 + 20) = 2000 for empty
            reader.write_unknown(count=2000)

            meshes_info.append(m)

        # 0 ?
        # write bones data
        # skipp for now - no bones

        # write mesh data
        for m in meshes_info:
            # write bone map list

            # write vertices
            for vert in m.vertices:
                reader.write_float32(vert.co.x)
                reader.write_float32(vert.co.y)
                reader.write_float32(vert.co.z)

            # write normals
            for vert in m.vertices:
                reader.write_float32(vert.normal.x)
                reader.write_float32(vert.normal.y)
                reader.write_float32(vert.normal.z)

            # write uv
            for uv in m.uv:
                reader.write_float32(uv[0])
                reader.write_float32(uv[1] if m.UVFLIP == False else 1.0 - uv[1])

            # write vertex color
            reader.write_unknown(count=12 * len(m.vertices))

            # write faces
            reader.write_unknown(count=12 * len(m.vertices))

            # rigged

            # write skin weights
            # write skin indices

            # write mesh indices 1164 * 2
            for face in m.triangles:
                reader.write_uint16(face[0])
                reader.write_uint16(face[1])
                reader.write_uint16(face[2])

            print(f'Completed: {reader.tell()}')

        # finish
        self.log('Export completed!', 'INFO')
        return ret


def save(operator, context, filepath="",
         use_selection=False,
         use_visible=False,
         **kwargs
         ):
    
    ret = {'FINISHED'}
    print("Exporting RMB file: " + filepath)

    basename = os.path.basename(filepath)
    filename, extension = os.path.splitext(basename)

    if extension == '.rmb':
        file = open(filepath, 'wb')
        reader = BinaryReader(file)			
        exporter = ExportRMB(operator)
        ret = exporter.parse(filename, reader)
        file.close()

    return ret
