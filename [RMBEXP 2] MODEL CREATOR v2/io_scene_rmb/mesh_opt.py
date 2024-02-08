import bpy
from bpy.types import Operator
from bpy.props import StringProperty


class RMBMeshToolPanel(bpy.types.Panel):
    bl_label = "RMB Mesh Tool"
    bl_idname = "RMB_PT_MeshToolPanel"
    bl_space_type = 'VIEW_3D'
    bl_region_type = 'UI'
    bl_category = 'RMB Tools'
    bl_description = "RMB Mesh Tool"
    

    def draw(self, context):
        layout = self.layout

        selected_object = context.active_object
        is_enabled = True if selected_object is not None and selected_object.type == 'MESH' else False

        if is_enabled:
            f_row = layout.row(align=True)
            f_row.operator("object.rmb_mesh_check_type", text="Check mesh type")
            # f_row.active = is_enabled

            t_row = layout.row(align=True)
            t_row.operator("object.rmb_mesh_split_by_faces", text="Split mesh by faces")
            # t_row.active = is_enabled

            s_row = layout.row(align=True)
            s_row.operator("object.rmb_mesh_opt_convert_to_tris", text="Convert mesh to triangles")
            # s_row.active = is_enabled
        else:
            layout.label(text="Please select a valid mesh object.")
    
class CheckMeshTypeOperator(bpy.types.Operator):
    bl_idname = "object.rmb_mesh_check_type"
    bl_label = "Check Mesh Type"
    bl_description = "Check mesh type, it's either Quad, Triangle or Mixed"

    def execute(self, context):
        selected_object = context.active_object
        if selected_object and selected_object.type == 'MESH':
            from .functionsLib import check_mesh_type
            mesh_type = check_mesh_type(selected_object)
            self.report({'INFO'}, f"{mesh_type}")
            return {'FINISHED'}
        else:
            self.report({'ERROR'}, "Please select a valid mesh object.")
            return {'CANCELLED'}
        

class SplitMeshByFacesOperator(bpy.types.Operator):
    bl_idname = "object.rmb_mesh_split_by_faces"
    bl_label = "Split Mesh by Faces"
    bl_description = "Split mesh by faces before converting to triangles, to fix the UV seams (RMB's specific)"

    def execute(self, context):
        selected_object = context.active_object
        if selected_object and selected_object.type == 'MESH':
            from .functionsLib import split_mesh_by_faces
            split_mesh_by_faces(selected_object)
            self.report({'INFO'}, "Mesh split by faces!")
            return {'FINISHED'}
        else:
            self.report({'ERROR'}, "Please select a valid mesh object.")
            return {'CANCELLED'}


class ConvertMeshToTrianglesOperator(bpy.types.Operator):
    bl_idname = "object.rmb_mesh_opt_convert_to_tris"
    bl_label = "Convert Mesh to Triangles"
    bl_description = "Convert mesh to triangles, to fix the UV seams (RMB's specific)"

    def execute(self, context):
        selected_object = context.active_object
        if selected_object and selected_object.type == 'MESH':
            from .functionsLib import convert_quads_to_triangles
            convert_quads_to_triangles(selected_object)
            self.report({'INFO'}, "Mesh converted to triangles!")
            return {'FINISHED'}
        else:
            self.report({'ERROR'}, "Please select a valid mesh object.")
            return {'CANCELLED'}
