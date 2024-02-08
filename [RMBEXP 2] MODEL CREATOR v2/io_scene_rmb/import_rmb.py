import os
from .meshLib import Mesh, Skin, Mat
from .skeletonLib import Skeleton, Bone
from .functionsLib import *
from .binaresLib import BinaryReader


class TexturePack():
	def __init__(self, diffuse=None, specular=None, normal=None):
		self.diffuse = diffuse
		self.specular = specular
		self.normal = normal


class ImportRMB():
	def __init__(self, operator):
		self.operator = operator

	def log(self, message, type='INFO'):
		self.operator.report({type}, message)

	def get_specific_texture(self, base_name: str, tex_type: str) -> str:
		filename = base_name.split('.')[0]
		ext = base_name.split('.')[-1]
		filename = filename + tex_type + '.' + ext

		if os.path.exists(filename):
			return filename
		else:
			self.log(f'\tTexture not found: {filename}', 'WARNING')
			return None

	def parse(self, filename: str, reader: BinaryReader):	
		ret = {'FINISHED'}
		
		# header - 36 bytes
		item_flag = reader.read_int32()         # 4 bytes item flag
		reader.read_unknown(count=16)           # unknown 16 bytes offset             
		texture_count = reader.read_int32()     # texture count
		mesh_count = reader.read_int32()        # mesh count
		bone_count = reader.read_int32()        # bone count
		data_offset = reader.read_int32()       # data offset

		# print header info
		print(f'Header:')
		print(f'\tItem flag: {item_flag}')
		print(f'\tTexture count: {texture_count}')
		print(f'\tMesh count: {mesh_count}')
		print(f'\tBone count: {bone_count}')

		texDir = reader.dirname.split('model')[0]+'texture'
		if os.path.exists(texDir) == False:
			texDir = reader.dirname
			
		# 260 bytes for each texture
		print(f'Textures: {texture_count}')
		textures = []
		for a in range(texture_count):
			texture = TexturePack() 

			texname = reader.read_string(limit=64)          # 64 bytes texture name
			texpath = texDir+os.sep+texname

			texture.diffuse = texpath
			
			print(f'Texture: {a}')
			print(f'\tDiffuse: {texname}')

			# get specular texture
			specular = self.get_specific_texture(texDir+os.sep+texname, '_sp')
			if specular is not None:
				texture.specular = specular
				print(f'\tSpecular: {specular}')

			# get normal texture
			normal = self.get_specific_texture(texDir+os.sep+texname, '_n')
			if normal is not None:
				texture.normal = normal
				print(f'\tNormal: {normal}')

			t_unknown = reader.read_unknown(count=196)      # unknown 196 bytes offset
			# print(f'\tTexture unknown: {reader.to_hex(t_unknown)}')

			textures.append(texture)
			
		# 2156 bytes for each mesh
		print(f'Meshes: {mesh_count}')
		meshes = []	
		for a in range(mesh_count):
			mesh = Mesh()	
			
			index = reader.read_int32()                     # 4 bytes index
			m_unknown = reader.read_unknown(count=4)        # unknown 4 bytes offset
			
			mesh.name = reader.read_string(limit=64)        # 64 bytes mesh name
			mesh.parentBone = reader.read_string(limit=64)  # 64 bytes parent bone name

			print(f'Mesh: {index}')
			print(f'\tUnknown: {reader.to_hex(m_unknown)}')							                    
			print(f'\tName: {mesh.name}')
			print(f'\tParent bone: {mesh.parentBone}')

			mesh.hasArmature = reader.read_int32() != 0     # rigged 4 bytes
			mesh.textureIndex = reader.read_int32()         # texture index 4 bytes
			mesh.boneMapCount = reader.read_int32()         # boneMapCount 4 bytes
			mesh.verticesCount = reader.read_int32()        # vertexCount 4 bytes
			mesh.indicesCount = reader.read_int32()         # indicesCount 4 bytes

			print(f'\tHasArmature: {mesh.hasArmature}')
			print(f'\tTextureIndex: {mesh.textureIndex}')
			print(f'\tBoneMapCount: {mesh.boneMapCount}')
			print(f'\tVertexCount: {mesh.verticesCount}')
			print(f'\tIndicesCount: {mesh.indicesCount}')

			# unknown data
			reader.read_unknown(count=2000)                 # unknown 2000 bytes
			meshes.append(mesh)
		

		# has armature
		skeleton = None
		if bone_count > 0:
			skeleton = Skeleton()
			skeleton.ARMATURESPACE = True
			#skeleton.BONESPACE = True
			skeleton.NICE = True
		
			# 412 bytes for each bone
			print(f'Bones: {bone_count}')
			for a in range(bone_count):
				bone = Bone()
				bone.ID = reader.read_int32()                               # 4 bytes ID
				bone.parentID = reader.read_int32()                         # 4 bytes parent ID

				reader.read_unknown(count=84)                               # unknown 84 bytes offset

				bone.name = reader.read_string(limit=64)                    # 64 bytes bone name
				# print(f'\tBone name: {bone.name}')
				
				bone.parentName = reader.read_string(limit=64)              # 64 bytes parent name
				# print(f'\tParent name: {bone.parentName}')
				
				m1 = Matrix4x4(reader.read_matrix4x4())                     # 16*4=64 bytes matrix
				m2 = Matrix4x4(reader.read_matrix4x4())                     # 16*4=64 bytes matrix
				m3 = Matrix4x4(reader.read_matrix4x4())
				bone.matrix = m3.inverted().transposed()                    # 16*4=64 bytes matrix
				skeleton.boneList.append(bone)

			skeleton.draw()	
			
		print(f'Meshes details: {mesh_count}')
		for mesh in meshes:
			if not isinstance(mesh, Mesh):
				continue

			# 1 * boenMapCount - bone mapping
			boneMapList = []
			for b in range(mesh.boneMapCount):
				boneMapList.append(reader.read_uint8())        # 1 bytes bone map
			boneMap = tuple(boneMapList)
															
			# vertices
			for b in range(mesh.verticesCount):                # 12 bytes for each vertex position
				x = reader.read_float32()                      # 4 bytes x
				y = reader.read_float32()                      # 4 bytes y
				z = reader.read_float32()                      # 4 bytes z
				mesh.vertPosList.append((x, y, z))

			# vertices normals
			for b in range(mesh.verticesCount):                # 12 bytes for each vertex normal
				x = reader.read_float32()                      # 4 bytes x
				y = reader.read_float32()                      # 4 bytes y
				z = reader.read_float32()                      # 4 bytes z
				mesh.vertNormList.append((x, y, z))

			# uv coordinates
			for b in range(mesh.verticesCount):                # 8 bytes for each vertex uv
				x = reader.read_float32()                      # 4 bytes x
				y = reader.read_float32()                      # 4 bytes y
				mesh.vertUVList.append((x, y))
			
			# unknown data                                     # vertexCount * 12
			reader.read_unknown(count=mesh.verticesCount*12)   # ???
				
			# unknown data                                     # 26476
			reader.read_unknown(count=mesh.verticesCount*12)   # ???


			# skin setup
			if mesh.hasArmature:                               
				for b in range(mesh.verticesCount):            # 16 bytes for each vertex skin weight
					x = reader.read_float32()                  # 4 bytes skin weight
					y = reader.read_float32()                  # 4 bytes skin weight
					z = reader.read_float32()                  # 4 bytes skin weight
					w = reader.read_float32()                  # 4 bytes skin weight
					mesh.skinWeightList.append((x, y, z, w))
					
				for b in range(mesh.verticesCount):            # 4 bytes for each vertex skin indice
					x = reader.read_uint8()                    # 1 byte skin indice
					y = reader.read_uint8()                    # 1 byte skin indice
					z = reader.read_uint8()                    # 1 byte skin indice
					w = reader.read_uint8()                    # 1 byte skin indice
					mesh.skinIndiceList.append((x, y, z, w))

				skin = Skin()
				skin.boneMap = boneMap
				mesh.skinList.append(skin)
				mesh.boneNameList = skeleton.boneNameList
			else:
				for b in range(mesh.verticesCount):
					mesh.skinWeightList.append([1.0])
					
				for b in range(mesh.verticesCount):
					mesh.skinIndiceList.append([0])
					
				skin = Skin()
				skin.boneMap=[0]
				mesh.skinList.append(skin)
				mesh.boneNameList = [mesh.parentBone]
			

			# indices
			for b in range(mesh.indicesCount):
				mesh.indiceList.append(reader.read_uint16())   # 2 bytes indice
			

			# setup mesh
			mesh.TRIANGLE = True
			# mesh.QUAD = True
			mesh.BINDSKELETON = skeleton.name if skeleton != None else None
			mesh.BINDPOSE = True
			mesh.UVFLIP = True
			
			# create material for mesh | usually 1 texture per mesh?
			mat = Mat()
			mat.TRIANGLE = True
			# mat.QUAD = True

			if mesh.textureIndex < len(textures):
				mat.diffuse = textures[mesh.textureIndex].diffuse
				mat.specular = textures[mesh.textureIndex].specular
				mat.normal = textures[mesh.textureIndex].normal
			else:
				self.log(f'Material index out of range: {mesh.textureIndex}', 'WARNING')

			mesh.matList.append(mat)

			# bind mesh matrix to bone matrix
			if skeleton != None:
				for bone in skeleton.boneList:
					if bone.name == mesh.parentBone:		
						mesh.matrix = skeleton.matrix @ bone.matrix

			mesh.draw()

		
		self.log(f'FIle total size: {reader.fileSize()}')

		# enable shading type MARETIAL
		for area in bpy.context.screen.areas: 
			if area.type == 'VIEW_3D':
				space = area.spaces.active
				if space.type == 'VIEW_3D':
					space.shading.type = 'MATERIAL'

		return ret


def load(operator, context, filepath="", 
		 axis_forward='-Z',
         axis_up='Y',
		 **kwargs
		 ):
	
	ret = {'FINISHED'}
	operator.report({'INFO'}, f'Importing RMB file: {filepath}')
    
	basename = os.path.basename(filepath)
	filename, extension = os.path.splitext(basename)

	if extension == '.rmb':
		file = open(filepath, 'rb')
		reader = BinaryReader(file)			
		importer = ImportRMB(operator)
		ret = importer.parse(filename, reader)
		file.close()

		if ret == {'FINISHED'}:
			operator.report({'INFO'}, f'Import RMB completed!')
		else:
			operator.report({'ERROR'}, f'Import RMB failed!')
	else:
		operator.report({'ERROR'}, f'Unsupported file extension: {extension}')
		ret = {'CANCELLED'}
	
	return ret