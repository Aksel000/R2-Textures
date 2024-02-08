import bpy
from mathutils import *
from .functionsLib import *
import random
import bmesh

from bpy_extras import node_shader_utils, image_utils


class Skin:
	def __init__(self):
		self.boneMap = []
		self.IDStart = None
		self.IDCount = None
		self.skeleton = None
		self.skeletonFile = None
			

class Mat:
	def __init__(self):
		self.name = None
		self.ZTRANS = False
		self.specular_value = 1.0
		self.roughness_value = 1.0
		
		self.diffuse = None
		self.DIFFUSESLOT = 0
		
		self.diffuse1 = None
		self.DIFFUSE1SLOT = 6
		self.diffuse2 = None
		self.DIFFUSE2SLOT = 7
		self.alpha = None
		self.ALPHASLOT = 8
		
		self.normal = None
		self.NORMALSLOT = 1
		self.NORMALSTRONG = 0.5
		self.NORMALDIRECTION = 1
		self.NORMALSIZE = (1,1,1) 
		
		self.specular = None
		self.SPECULARSLOT = 2
		
		self.ao = None
		self.AOSLOT = 3
		
		self.normal1 = None
		self.NORMAL1SLOT = 4
		self.NORMAL1STRONG = 0.8
		self.NORMAL1DIRECTION = 1
		self.NORMAL1SIZE = (15,15,15) 
		
		self.normal2 = None
		self.NORMAL2SLOT = 5
		self.NORMAL2STRONG = 0.8
		self.NORMAL2DIRECTION = 1
		self.NORMAL2SIZE = (15,15,15) 
		
		self.reflection=None
		self.REFLECTIONSLOT=8
		self.REFLECTIONSTRONG=1.0
		
		#self.USEDTRIANGLES = [None,None]
		self.TRIANGLE = False
		self.TRISTRIP = False
		self.QUAD = False
		self.IDStart = None
		self.IDCount = None
		self.faceIDList = []
		
		r = random.randint(0, 255)
		g = random.randint(0, 255)
		b = random.randint(0, 255)
		self.rgba = [r/255.0, g/255.0, b/255.0,1.0]
		
	def draw(self): 
		if self.name is None:
			self.name = str(ParseID())+'-mat-'+str(0)

		blendMat = bpy.data.materials.new(name=self.name)
		blendMat.use_nodes = True
		node_principled = next(node for node in blendMat.node_tree.nodes if node.type == 'BSDF_PRINCIPLED')
		node_principled.inputs['Roughness'].default_value = 0.5
		node_principled.inputs['Specular'].default_value = 0.5
		node_principled.inputs['Roughness'].default_value = 0.04

		# blendMat = Blender.Material.New(self.name)
		# blendMat.diffuseShader=Blender.Material.Shaders.DIFFUSE_ORENNAYAR
		# blendMat.specShader=Blender.Material.Shaders.SPEC_WARDISO
		# blendMat.setRms(0.04)
		# blendMat.shadeMode=Blender.Material.ShadeModes.CUBIC
		if self.ZTRANS == True:
			blendMat.blend_method = 'BLEND'
			node_principled = next(node for node in blendMat.node_tree.nodes if node.type == 'BSDF_PRINCIPLED')
			node_principled.inputs['Transmission'].default_value = 1.0
			node_principled.inputs['Alpha'].default_value = 0.0
			# blendMat.mode |= Blender.Material.Modes.ZTRANSP
			# blendMat.mode |= Blender.Material.Modes.TRANSPSHADOW
			# blendMat.alpha = 0.0 

		if self.diffuse is not None:
			diffuse(blendMat, self)
		if self.reflection is not None:
			reflection(blendMat, self)
		if self.diffuse1 is not None:
			diffuse1(blendMat, self)
		if self.diffuse2 is not None:
			diffuse2(blendMat, self)
		if self.specular is not None:
			specular(blendMat, self)
		if self.normal is not None: 
			normal(blendMat, self)
		if self.normal1 is not None:
			normal1(blendMat, self)
		if self.normal2 is not None:
			normal2(blendMat, self)
		if self.ao is not None:
			ao(blendMat, self)
		if self.alpha is not None:
			alpha(blendMat, self)

			
class Mesh():
	def __init__(self):
		self.vertPosList = []
		self.vertNormList = []
		self.vertIndexList = []
		
		self.indiceList = []
		self.faceList = []          # always empty ?
		self.triangleList = []
		
		self.matList = []           # always empty ?
		self.matIDList = []
		self.vertUVList = []
		self.faceUVList = []
		
		self.skinList = []
		self.skinWeightList = []
		self.skinIndiceList = []
		self.skinIDList = []
		self.bindPoseMatrixList = []
		self.boneNameList = []
		
		# info
		self.hasArmature = False
		self.textureIndex = 0
		self.boneMapCount = 0
		self.verticesCount = 0
		self.indicesCount = 0

		self.parentBone = None

		self.name = None
		self.mesh = None
		self.object = None
		self.TRIANGLE = False
		self.QUAD = False
		self.TRISTRIP = False
		self.BINDSKELETON = None
		self.BINDPOSESKELETON = None
		self.matrix = None
		self.SPLIT = False
		self.WARNING = False
		self.DRAW = False
		self.SETBOX = None
		self.BINDPOSE = False
		self.UVFLIP = False
		
	def log(self, message, type='INFO'):
		if self.WARNING == True:
			print(f'{type}: {message}')

	def update(self):
		pass
		
	def setBox(self):
		E=[[],[],[]]
		for n in range(len(self.vertPosList)):
			x,y,z = self.vertPosList[n]
			E[0].append(x)
			E[1].append(y)
			E[2].append(z)	

		skX = (self.SETBOX[3]-self.SETBOX[0])/(max(E[0])-min(E[0]))
		skY = (self.SETBOX[4]-self.SETBOX[1])/(max(E[1])-min(E[1]))
		skZ = (self.SETBOX[5]-self.SETBOX[2])/(max(E[2])-min(E[2]))
		sk = min(skX,skY,skZ)
		trX = (self.SETBOX[3]+self.SETBOX[0])/2
		trY = (self.SETBOX[4]+self.SETBOX[1])/2
		trZ = (self.SETBOX[5]+self.SETBOX[2])/2
		
		
		for n in range(len(self.vertPosList)):
			x,y,z = self.vertPosList[n]
			self.vertPosList[n] = [trX+x*skX,trY+y*skY,trZ+z*skZ]
			
	def addMaterial(self, mat, mesh, matID):
		if mat.name is None:
			mat.name = self.name+'-mat-'+str(matID)

		blendMat = bpy.data.materials.new(name=mat.name)

		mat_wrap = node_shader_utils.PrincipledBSDFWrapper(blendMat, is_readonly=False, use_nodes=True)
		# mat_wrap.base_color = mat.rgba[:3] # + [1.0]  # RGBA
		mat_wrap.alpha = mat.rgba[3]
		mat_wrap.specular = mat.specular_value
		mat_wrap.roughness = mat.roughness_value

		if mat.ZTRANS == True:
			blendMat.blend_method = 'BLEND'
			blendMat.shadow_method = 'HASHED'

		if mat.diffuse is not None:
			mat_wrap.base_color_texture.image = image_utils.load_image(mat.diffuse)
		if mat.specular is not None:
			mat_wrap.specular_texture.image = image_utils.load_image(mat.specular)
		if mat.normal is not None:
			mat_wrap.normalmap_texture.image = image_utils.load_image(mat.normal)

		# if mat.reflection is not None:
		# 	reflection(blendMat, mat)
		# if mat.diffuse1 is not None:
		# 	diffuse1(blendMat, mat)
		# if mat.diffuse2 is not None:
		# 	diffuse2(blendMat, mat)
		# if mat.normal1 is not None:
		# 	normal1(blendMat, mat)
		# if mat.normal2 is not None:
		# 	normal2(blendMat, mat)
		# if mat.ao is not None:
		# 	ao(blendMat, mat)
		# if mat.alpha is not None:
		# 	alpha(blendMat, mat)

		# build nodes
		mat_wrap.update()

		nodes = blendMat.node_tree.nodes

		# setup uv map for diffuse texture
		texture_node = nodes.get('Image Texture')
		if texture_node is not None:
			uvmap_node = nodes.new(type='ShaderNodeUVMap')
			blendMat.node_tree.links.new(texture_node.inputs['Vector'], uvmap_node.outputs['UV'])

		# setup emission
		node_principled = next(node for node in nodes if node.type == 'BSDF_PRINCIPLED')
		if texture_node is not None:
			emission_input = node_principled.inputs.get('Emission')
			# support blender 4.0
			if emission_input is None:
				emission_input = node_principled.inputs.get('Emission Color')

			if emission_input is not None:
				blendMat.node_tree.links.new(emission_input, texture_node.outputs['Color'])
			else:
				self.log('Emission input not found', 'WARNING')

		# add material to mesh
		mesh.materials.append(blendMat)

		# logs
		self.log(f'Mesh.Material.name: {mat.name}')
		self.log(f'Mesh.Material.ZTRANS: {mat.ZTRANS}')
		if mat.diffuse is not None:
			self.log(f'Mesh.Material.diffuse: {mat.diffuse}')
		if mat.specular is not None:
			self.log(f'Mesh.Material.specular: {mat.specular}')
		if mat.normal is not None:
			self.log(f'Mesh.Material.normal: {mat.normal}')
		
	def addVertexUV(self, blenderMesh, mesh): 
		# create new layer for UV coordinates
		uv_layer = blenderMesh.uv_layers.new()
		bm = bmesh.new()
		bm.from_mesh(blenderMesh)

		bm.faces.ensure_lookup_table()
		bm.loops.layers.uv.verify()

		uv_layer = bm.loops.layers.uv.active

		# set the UV coordinates of the loops
		for face in bm.faces:
			for loop in face.loops:
				i = loop.vert.index
				if self.UVFLIP == False:
					loop[uv_layer].uv = Vector(mesh.vertUVList[i])
				else:
					loop[uv_layer].uv = Vector((mesh.vertUVList[i][0], 1.0 - mesh.vertUVList[i][1]))

		# Update the BMesh to the mesh
		bm.to_mesh(blenderMesh)
		bm.free()
		
	def addFaceUV(self, blenderMesh, mesh):
		self.log(f'Mesh.blenderMesh.faces: {len(blenderMesh.polygons)}', 'WARNING')

		if len(blenderMesh.polygons) > 0:
			bm = bmesh.new()
			bm.from_mesh(blenderMesh)

			bm.faces.ensure_lookup_table()
			bm.verts.ensure_lookup_table()
			bm.loops.layers.uv.verify()

			for face in bm.faces:
				face.smooth = True

				if len(mesh.matIDList) > 0:
					face.material_index = mesh.matIDList[face.index]
			
			bm.to_mesh(blenderMesh)
			bm.free()

			blenderMesh.update(calc_edges=True)

	def addSkinIDList(self):
		two_spaces = '\t'*2
		if len(self.skinIDList) == 0:
			for skinID in range(len(self.skinList)):
				skin = self.skinList[skinID]
				
				if skin.IDStart == None:
					skin.IDStart = 0
				
				if skin.IDCount == None:
					skin.IDCount = len(self.skinIndiceList)
				
				for vertID in range(skin.IDCount):
					self.skinIDList.append(skinID)
				
				self.log(f'\tMesh.Skin.boneMap: {len(skin.boneMap)}')
				self.log(f'{two_spaces}Mesh.Skin.IDStart: {skin.IDStart}')
				self.log(f'{two_spaces}Mesh.Skin.IDCount: {skin.IDCount}')
				self.log(f'{two_spaces}Mesh.Skin.skinIDList: {len(self.skinIDList)}')
		else:
			self.log(f'{two_spaces}Mesh.Skin.skinIDList: {len(self.skinIDList)}')
			for skinID in range(len(self.skinList)):
				skin = self.skinList[skinID]
				self.log(f'\tMesh.Skin.boneMap: {len(skin.boneMap)}')
				
	def addSkin(self, blendMesh, mesh):	
		for vertID in range(len(mesh.skinIDList)):
			indices = mesh.skinIndiceList[vertID]
			weights = mesh.skinWeightList[vertID]
			skinID = mesh.skinIDList[vertID]

			for n in range(len(indices)):
				w  = weights[n]
				if type(w) == int:
					w = w / 255.0

				if w != 0:
					grID = indices[n]
					if len(self.boneNameList) == 0:
						if len(self.skinList[skinID].boneMap) > 0:
							grName = str(self.skinList[skinID].boneMap[grID])
						else:	
							grName = str(grID)
					else:	
						if len(self.skinList[skinID].boneMap) > 0:
							grNameID = self.skinList[skinID].boneMap[grID]
							grName = self.boneNameList[grNameID]
						else:	
							grName = self.boneNameList[grID]
					
					if grName not in [group.name for group in mesh.object.vertex_groups]:
						mesh.object.vertex_groups.new(name=grName)

					mesh.object.vertex_groups[grName].add([vertID], w, 'REPLACE')
				
	def addBindPose(self, blendMesh, mesh):
		poseBones = None
		poseSkeleton = None
		bindBones = None
		bindSkeleton = None

		if self.BINDPOSESKELETON is not None:
			scene = bpy.context.scene
			for object in scene.objects:
				if object.name == self.BINDPOSESKELETON:
					poseBones = object.data.bones
					poseSkeleton = object

		if self.BINDSKELETON is not None:
			scene = bpy.context.scene
			for object in scene.objects:
				if object.name == self.BINDSKELETON:
					bindBones = object.data.bones
					bindSkeleton = object

		if poseBones is not None and bindBones is not None:					
			for vert in blendMesh.vertices:
				index = vert.index
				skinList = mesh.object.vertex_groups[index].weight(vert)
				vco = vert.co.copy() @ self.object.matrix_world
				vector = Vector()

				for skin in skinList:
					bone = skin[0]							
					weight = skin[1]
					
					matB = bindBones[bone].matrix_basis @ bindSkeleton.matrix_world
					matA = poseBones[bone].matrix_basis @ poseSkeleton.matrix_world

					vector += vco @ matA.inverted() @ matB @ weight
					
				vert.co = vector
				
			blendMesh.update()	
				
	def addFaces(self): 
		self.log(f'\tMesh.matList count: {len(self.matList)}')
		for matID in range(len(self.matList)):
			two_spaces='\t'*2
			thre_spaces='\t'*3
			self.log(f'{two_spaces}Mesh.Material.name: {self.matList[matID].name}')
			self.log(f'{thre_spaces}Mesh.Material.IDStart: {self.matList[matID].IDStart}')
			self.log(f'{thre_spaces}Mesh.Material.IDCount: {self.matList[matID].IDCount}')

		if len(self.matList) == 0:
			if len(self.faceList) != 0:
				self.triangleList = self.faceList

			if len(self.indiceList) != 0:
				if self.TRIANGLE == True:
					self.indicesToTriangles(self.indiceList, 0)
				elif self.QUAD == True:
					self.indicesToQuads(self.indiceList, 0)
				elif self.TRISTRIP == True:
					self.indicesToTriangleStrips(self.indiceList, 0)
				else:
					self.log(f'Mesh.TRIANGLE: {self.TRIANGLE}', 'WARNING')
					self.log(f'Mesh.TRISTRIP: {self.TRISTRIP}', 'WARNING')
		else:
			if len(self.faceList) > 0:
				if len(self.matIDList) == 0:
					for matID in range(len(self.matList)):
						mat = self.matList[matID] 
						if mat.IDStart is not None and mat.IDCount is not None:
							for faceID in range(mat.IDCount):
								self.triangleList.append(self.faceList[mat.IDStart+faceID])
								self.matIDList.append(matID)
						else:
							if mat.IDStart == None:
								mat.IDStart = 0
							if mat.IDCount == None:
								mat.IDCount = len(self.faceList)
							for faceID in range(mat.IDCount):
								self.triangleList.append(self.faceList[mat.IDStart+faceID])
								self.matIDList.append(matID)
				else:			
					self.triangleList = self.faceList
						
			if len(self.indiceList) > 0:
				for matID in range(len(self.matList)):
					mat = self.matList[matID] 
					
					if mat.IDStart == None:
						mat.IDStart = 0
					if mat.IDCount == None:
						mat.IDCount = len(self.indiceList)
					
					indiceList = self.indiceList[mat.IDStart:mat.IDStart+mat.IDCount]					
					
					if mat.TRIANGLE == True:
						self.indicesToTriangles(indiceList, matID)
					elif mat.QUAD == True:
						self.indicesToQuads(indiceList, matID)
					elif mat.TRISTRIP == True:
						self.indicesToTriangleStrips(indiceList, matID)	
					
		self.log(f'\tMesh.triangleList count: {len(self.triangleList)}')
		self.log(f'\tMesh.matIDList count: {len(self.matIDList)}')

	def buildMesh(self, mesh, mat, meshID):
		self.log(f'Mesh.name: {mesh.name}')
		self.log(f'\tMesh.vertPosList count: {len(mesh.vertPosList)}')
		self.log(f'\tMesh.vertUVList count: {len(mesh.vertUVList)}')
		self.log(f'\tMesh.triangleList count: {len(mesh.triangleList)}')
		self.log(f'\tMesh.indiceList count: {len(mesh.indiceList)}')

		blendMesh = bpy.data.meshes.new(mesh.name)
		blendMesh.from_pydata(mesh.vertPosList, [], mesh.triangleList)
		self.addMaterial(mat, blendMesh, meshID)

		if len(mesh.triangleList) > 0:	
			if len(mesh.vertUVList) > 0:
				self.addVertexUV(blendMesh, mesh)
				self.addFaceUV(blendMesh, mesh)
			
			if len(mesh.faceUVList) > 0:
				self.addFaceUV(blendMesh,mesh)

		if len(mesh.vertNormList) > 0:
			for i,vert in enumerate(blendMesh.vertices):
				vert.normal = Vector(self.vertNormList[i])
			
		self.addSkin(blendMesh, mesh) # TODO: move afte ...objects.link ?

		scene = bpy.context.scene
		meshobject = bpy.data.objects.new(mesh.name, blendMesh)
		bpy.context.collection.objects.link(meshobject)

		if self.BINDSKELETON is not None:
			for object in scene.objects:
				if object.name == self.BINDSKELETON:
					skeletonMatrix = meshobject.matrix_world @ object.matrix_world
					meshobject.matrix_world = skeletonMatrix
					self.object.parent = object
					self.object.parent_type = 'ARMATURE'

		if self.matrix is not None:
			meshobject.matrix_world = self.matrix @ meshobject.matrix_world
		
	def addMesh(self):
		self.mesh = bpy.data.meshes.new(self.name)
		self.mesh.from_pydata(self.vertPosList, [], self.triangleList)
		self.mesh.update()

		# TODO ?
		# if len(self.vertNormList) > 0:
		# 	self.mesh.vertices.foreach_set('normal', [n for v in self.vertNormList for n in v])

		self.mesh.update(calc_edges=True)
		self.mesh.validate(verbose=True)

		# add to scene
		self.object = bpy.data.objects.new(self.name, self.mesh)
		bpy.context.collection.objects.link(self.object)

		# Select and make active
		bpy.context.view_layer.objects.active = self.object
		self.object.select_set(True)

		# Smooth shading
		bpy.ops.object.shade_smooth()
		
	def boneTree(self,parent):
		for bone in parent.children:
			self.boneTree(bone)
			
	def draw(self): 
		if self.name is None:
			self.name = str(ParseID())+'-model-'+str(0)

		self.log(f'Mesh.name: {self.name}')
		self.log(f'\tMesh.vertPosList count: {len(self.vertPosList)}')
		self.log(f'\tMesh.vertUVList count: {len(self.vertUVList)}')
		self.log(f'\tMesh.indiceList count: {len(self.indiceList)}')
		self.log(f'\tMesh.faceList count: {len(self.faceList)}')		
		self.log(f'\tMesh.triangleList count: {len(self.triangleList)}')
		self.log(f'\tMesh.faceUVList count: {len(self.faceUVList)}')
		
		self.addFaces() 
			
		self.log(f'\tMesh.SPLIT: {self.SPLIT}')

		self.addSkinIDList()

		if self.SETBOX is not None:
			self.setBox()
			
		# split FALSE
		if self.SPLIT == False:
			# create mesh
			self.addMesh()	

			# add vertex uv
			if len(self.triangleList) > 0:	
				if len(self.vertUVList) > 0:
					self.addVertexUV(self.mesh, self)

			# add faces
			self.addFaceUV(self.mesh, self)

			# add materials
			for matID in range(len(self.matList)):
				mat = self.matList[matID]
				self.addMaterial(mat, self.mesh, matID)
				
			# add skeleton
			if self.BINDSKELETON is not None:
				scene = bpy.context.scene
				for object in scene.objects:
					if object.name == self.BINDSKELETON:
						skeletonMatrix = self.object.matrix_world @ object.matrix_world
						self.object.matrix_world = skeletonMatrix
						self.object.parent = object
						self.object.parent_type = 'ARMATURE'
			
			# add skin
			self.addSkin(self.mesh, self)
			
			# set matrix
			if self.matrix is not None:
				self.object.matrix_world = self.matrix @ self.object.matrix_world

			# add bind pose	
			if self.BINDPOSE == True:
				self.addBindPose(self.mesh, self)

		# split TRUE
		else:		
			self.log('SPLIT:True')
			self.log('MESH SPLITING PROCCES:')

			meshList=[]

			for matID in range(len(self.matList)):
				mesh = Mesh()
				mesh.idList = {}
				mesh.id = 0
				mesh.name = self.name+'-'+str(matID)
				meshList.append(mesh)

				for n in range(len(self.vertPosList)):
					mesh.idList[str(n)] = None					
					
			for faceID in range(len(self.matIDList)):
				matID = self.matIDList[faceID]
				mesh = meshList[matID]
				face = []

				for v in range(len(self.triangleList[faceID])):
					vid = self.triangleList[faceID][v]

					if mesh.idList[str(vid)] == None:	  
						mesh.idList[str(vid)] = mesh.id
						mesh.vertPosList.append(self.vertPosList[vid])
						
						if len(self.vertUVList) > 0:
							mesh.vertUVList.append(self.vertUVList[vid]) 
						if len(self.vertNormList)>0:
							mesh.vertNormList.append(self.vertNormList[vid]) 
						
						if len(self.skinIndiceList)>0 and len(self.skinWeightList)>0:	
							mesh.skinWeightList.append(self.skinWeightList[vid])
							mesh.skinIndiceList.append(self.skinIndiceList[vid])
							mesh.skinIDList.append(self.skinIDList[vid])	

						face.append(mesh.id) 
						mesh.id += 1
					else:
						oldid = mesh.idList[str(vid)] 
						face.append(oldid) 

				mesh.triangleList.append(face)

				if len(self.faceUVList)>0:
					mesh.faceUVList.append(self.faceUVList[faceID]) 

				mesh.matIDList.append(0)
				
			for meshID in range(len(meshList)):
				mesh = meshList[meshID]
				mat = self.matList[meshID]
				self.buildMesh(mesh, mat, meshID)
							
	def indicesToQuads(self, indicesList, matID):
		for m in range(0, len(indicesList), 4):
			self.triangleList.append(indicesList[m:m+4] )
			self.matIDList.append(matID)
				
	def indicesToTriangles(self, indicesList, matID):
		for m in range(0, len(indicesList), 3):
			self.triangleList.append(indicesList[m:m+3])
			self.matIDList.append(matID)
		
	def indicesToTriangleStrips(self, indicesList, matID):
		StartDirection = -1
		id = 0
		f1 = indicesList[id]
		id += 1
		f2 = indicesList[id]
		FaceDirection = StartDirection

		while(True):
		#for m in range(len(indicesList)-2):
			id += 1
			f3 = indicesList[id]
			#print f3
			if (f3 == 0xFFFF):
				if id == len(indicesList)-1:
					break

				id += 1
				f1 = indicesList[id]
				id += 1
				f2 = indicesList[id]
				FaceDirection = StartDirection	 
			else:
				#f3 += 1
				FaceDirection *= -1
				if (f1 != f2) and (f2 != f3) and (f3 != f1):
					if FaceDirection > 0:						
						self.triangleList.append([(f1),(f2),(f3)])
						self.matIDList.append(matID)
					else:
						self.triangleList.append([(f1),(f3),(f2)])
						self.matIDList.append(matID)
					if self.DRAW == True: 
						f1,f2,f3	
				f1 = f2
				f2 = f3

			if id == len(indicesList)-1:
				break


def diffuse(blendMat, data, mesh=None):
	if os.path.exists(data.diffuse) == True:

		blendMat.use_nodes = True
		material_node = next(node for node in blendMat.node_tree.nodes if node.type == 'BSDF_PRINCIPLED')
		texture_node = blendMat.node_tree.nodes.new(type='ShaderNodeTexImage')
		texture_node.image = bpy.data.images.load(data.diffuse)

		uv_node = blendMat.node_tree.nodes.new(type='ShaderNodeUVMap')
		blendMat.node_tree.links.new(material_node.inputs['Base Color'], texture_node.outputs['Color'])
		blendMat.node_tree.links.new(material_node.inputs['Alpha'], texture_node.outputs['Alpha'])
		blendMat.node_tree.links.new(texture_node.inputs['Vector'], uv_node.outputs['UV'])
		texture_node.extension = 'REPEAT'

		# hack to assign right uv map
		if mesh is not None:
			uv_maps = mesh.uv_layers
			if len(uv_maps) > 1:
				# get second uv map
				uv_map = uv_maps[1]
				uv_node.uv_map = uv_map.name

		# blendMat.setTexture(data.DIFFUSESLOT,tex,Blender.Texture.TexCo.UV, Blender.Texture.MapTo.COL| Blender.Texture.MapTo.ALPHA|Blender.Texture.MapTo.CSP)	
	else:
		# if self.WARNING == True:
		print(f'Diffuse load failed {data.diffuse}')
			
def reflection(blendMat, data):
	if os.path.exists(data.reflection) == True:
		img = bpy.data.images.load(data.reflection)
		imgName = blendMat.name.replace('-mat-','-refl-')
		img.name = imgName
		# texname = blendMat.name.replace('-mat-','-refl-')
		# tex = bpy.data.textures.new(name=texname, type='IMAGE')
		# tex.image = img 

		blendMat.use_nodes = True
		tex_node = blendMat.node_tree.nodes.new('ShaderNodeTexImage')
		tex_node.image = img

		node_principled = next(node for node in blendMat.node_tree.nodes if node.type == 'BSDF_PRINCIPLED')
		blendMat.node_tree.links.new(tex_node.outputs['Color'], node_principled.inputs['Base Color'])
		blendMat.node_tree.links.new(tex_node.outputs['Alpha'], node_principled.inputs['Alpha'])
		# blendMat.setTexture(data.REFLECTIONSLOT,tex,Blender.Texture.TexCo.REFL,Blender.Texture.MapTo.COL)	
			
		mtextures = [node for node in blendMat.node_tree.nodes if node.type == 'TEX_IMAGE']
		mtex = mtextures[data.REFLECTIONSLOT]
		mtex.inputs['Color'].default_value = data.REFLECTIONSTRONG
	#else:
	#	if self.WARNING==True:
	#		print 'failed...',data.reflection
			
def alpha(blendMat, data):
		if os.path.exists(data.alpha) == True:
			img = bpy.data.images.load(data.alpha)
			imgName = blendMat.name.replace('-mat-','-alpha-')
			img.name = imgName
			# texname = blendMat.name.replace('-mat-','-alpha-')
			# tex = bpy.data.textures.new(name=texname, type='IMAGE')
			# tex.image = img
			# tex.image.alpha_mode = 'CHANNEL_PACKED'

			blendMat.use_nodes = True
			tex_node = blendMat.node_tree.nodes.new('ShaderNodeTexImage')
			tex_node.image = img
			node_principled = next(node for node in blendMat.node_tree.nodes if node.type == 'BSDF_PRINCIPLED')
			blendMat.node_tree.links.new(tex_node.outputs['Alpha'], node_principled.inputs['Alpha'])
			# blendMat.setTexture(data.ALPHASLOT,tex,Blender.Texture.TexCo.UV, Blender.Texture.MapTo.ALPHA)
			#blendMat.getTextures()[data.DIFFUSESLOT].mtAlpha=0 
		#else:
		#	if self.WARNING==True:
		#		print 'failed...',data.diffuse
			
def diffuse1(blendMat, data):
		if os.path.exists(data.diffuse1) == True:
			img = bpy.data.images.load(data.diffuse1)
			imgName = blendMat.name.replace('-mat-','-diff-')
			img.name = imgName
			# texname=blendMat.name.replace('-mat-','-diff-')
			# tex = Blender.Texture.New(texname)
			# tex.setType('Image')
			# tex.image = img 

			blendMat.use_nodes = True
			tex_node = blendMat.node_tree.nodes.new('ShaderNodeTexImage')
			tex_node.image = img
			node_principled = next(node for node in blendMat.node_tree.nodes if node.type == 'BSDF_PRINCIPLED')
			blendMat.node_tree.links.new(tex_node.outputs['Color'], node_principled.inputs['Base Color'])
			blendMat.node_tree.links.new(tex_node.outputs['Color'], node_principled.inputs['Specular'])
			# blendMat.setTexture(data.DIFFUSE1SLOT,tex,Blender.Texture.TexCo.UV,Blender.Texture.MapTo.COL|Blender.Texture.MapTo.CSP)
		#else:
		#	if self.WARNING==True:
		#		print 'failed...',data.diffuse1
			
def diffuse2(blendMat, data):
		if os.path.exists(data.diffuse2) == True:
			img = bpy.data.images.load(data.diffuse2)
			imgName = blendMat.name.replace('-mat-','-diff-')
			img.name = imgName
			# texname=blendMat.name.replace('-mat-','-diff-')
			# tex = Blender.Texture.New(texname)
			# tex.setType('Image')
			# tex.image = img 

			blendMat.use_nodes = True
			tex_node = blendMat.node_tree.nodes.new('ShaderNodeTexImage')
			tex_node.image = img
			node_principled = next(node for node in blendMat.node_tree.nodes if node.type == 'BSDF_PRINCIPLED')
			blendMat.node_tree.links.new(tex_node.outputs['Color'], node_principled.inputs['Base Color'])
			blendMat.node_tree.links.new(tex_node.outputs['Color'], node_principled.inputs['Specular'])
			# blendMat.setTexture(data.DIFFUSE2SLOT,tex,Blender.Texture.TexCo.UV,Blender.Texture.MapTo.COL|Blender.Texture.MapTo.CSP)
		#else:
		#	if self.WARNING==True:
		#		print 'failed...',data.diffuse1
			
def normal(blendMat,data): 
		if os.path.exists(data.normal) == True:
			img = bpy.data.images.load(data.normal)
			imgName = blendMat.name.replace('-mat-','-norm-')
			img.name = imgName
			# texname=blendMat.name.replace('-mat-','-norm-')
			# tex = Blender.Texture.New(texname)
			# tex.setType('Image')
			# tex.image = img 
			# tex.setImageFlags('NormalMap')

			blendMat.use_nodes = True
			tex_node = blendMat.node_tree.nodes.new('ShaderNodeTexImage')
			tex_node.image = img

			normal_map_node = blendMat.node_tree.nodes.new('ShaderNodeNormalMap')
			normal_map_node.inputs['Strength'].default_value = data.NORMALSTRONG

			blendMat.node_tree.links.new(tex_node.outputs['Color'], normal_map_node.inputs['Color'])
			node_principled = next(node for node in blendMat.node_tree.nodes if node.type == 'BSDF_PRINCIPLED')
			blendMat.node_tree.links.new(normal_map_node.outputs['Normal'], node_principled.inputs['Normal'])
			# blendMat.setTexture(data.NORMALSLOT,tex,Blender.Texture.TexCo.UV,Blender.Texture.MapTo.NOR)

			mapping_node = blendMat.node_tree.nodes.new('ShaderNodeMapping')
			mapping_node.inputs['Scale'].default_value = data.NORMALSIZE
			blendMat.node_tree.links.new(mapping_node.outputs['Vector'], normal_map_node.inputs['Vector'])

			# blendMat.getTextures()[data.NORMALSLOT].norfac=data.NORMALSTRONG 
			# blendMat.getTextures()[data.NORMALSLOT].mtNor=data.NORMALDIRECTION 
			# blendMat.getTextures()[data.NORMALSLOT].size=data.NORMALSIZE
		#else:
		#	if self.WARNING==True:
		#		print 'failed...',data.normal 
		
def specular(blendMat, data):
		if os.path.exists(data.specular) == True:
			img = bpy.data.images.load(data.specular)
			imgName = blendMat.name.replace('-mat-','-spec-')
			img.name = imgName
			# texname=blendMat.name.replace('-mat-','-spec-')
			# tex = Blender.Texture.New(texname)
			# tex.setType('Image')
			# tex.image = img 

			blendMat.use_nodes = True
			tex_node = blendMat.node_tree.nodes.new('ShaderNodeTexImage')
			tex_node.image = img

			invert_node = blendMat.node_tree.nodes.new('ShaderNodeInvert')
			blendMat.node_tree.links.new(tex_node.outputs['Color'], invert_node.inputs['Color'])
			node_principled = next(node for node in blendMat.node_tree.nodes if node.type == 'BSDF_PRINCIPLED')
			blendMat.node_tree.links.new(invert_node.outputs['Color'], node_principled.inputs['Specular'])

			# blendMat.setTexture(data.SPECULARSLOT,tex,Blender.Texture.TexCo.UV,Blender.Texture.MapTo.CSP)	
			# mtextures = blendMat.getTextures() 
			# mtex=mtextures[data.SPECULARSLOT]
			# mtex.neg=True
		#else:
		#	if self.WARNING==True:
		#		print 'failed...',data.specular		
			
def ao(blendMat, data):
		if os.path.exists(data.ao) == True:
			img = bpy.data.images.load(data.ao)
			imgName = blendMat.name.replace('-mat-','-ao-')
			img.name = imgName
			# texname=blendMat.name.replace('-mat-','-ao-')
			# tex = Blender.Texture.New(texname)
			# tex.setType('Image')
			# tex.image = img 

			blendMat.use_nodes = True
			tex_node = blendMat.node_tree.nodes.new('ShaderNodeTexImage')
			tex_node.image = img
			mix_node = blendMat.node_tree.nodes.new('ShaderNodeMixRGB')
			mix_node.blend_type = 'MULTIPLY'

			blendMat.node_tree.links.new(tex_node.outputs['Color'], mix_node.inputs['Color1'])
			node_principled = next(node for node in blendMat.node_tree.nodes if node.type == 'BSDF_PRINCIPLED')
			blendMat.node_tree.links.new(mix_node.outputs['Color'], node_principled.inputs['Base Color'])

			# blendMat.setTexture(data.AOSLOT,tex,Blender.Texture.TexCo.UV,Blender.Texture.MapTo.COL) 
			# mtex=blendMat.getTextures()[data.AOSLOT]
			# mtex.blendmode=Blender.Texture.BlendModes.MULTIPLY
		#else:
		#	if self.WARNING==True:
		#		print 'failed...',data.ao	

def normal1(blendMat, data): 
		if os.path.exists(data.normal1) == True:
			img = bpy.data.images.load(data.normal1)
			imgName = blendMat.name.replace('-mat-','-norm1-')
			img.name = imgName
			# texname=blendMat.name.replace('-mat-','-norm1-')
			# tex = Blender.Texture.New(texname)
			# tex.setType('Image')
			# tex.image = img 
			# tex.setImageFlags('NormalMap')

			blendMat.use_nodes = True
			tex_node = blendMat.node_tree.nodes.new('ShaderNodeTexImage')
			tex_node.image = img

			normal_map_node = blendMat.node_tree.nodes.new('ShaderNodeNormalMap')
			normal_map_node.inputs['Strength'].default_value = data.NORMAL1STRONG

			mapping_node = blendMat.node_tree.nodes.new('ShaderNodeMapping')
			mapping_node.inputs['Scale'].default_value = data.NORMAL1SIZE

			blendMat.node_tree.links.new(mapping_node.outputs['Vector'], tex_node.inputs['Vector'])
			blendMat.node_tree.links.new(tex_node.outputs['Color'], normal_map_node.inputs['Color'])

			node_principled = next(node for node in blendMat.node_tree.nodes if node.type == 'BSDF_PRINCIPLED')
			blendMat.node_tree.links.new(normal_map_node.outputs['Normal'], node_principled.inputs['Normal'])
			# blendMat.setTexture(data.NORMAL1SLOT,tex,Blender.Texture.TexCo.UV,Blender.Texture.MapTo.NOR)
			# blendMat.getTextures()[data.NORMAL1SLOT].norfac=data.NORMAL1STRONG 
			# blendMat.getTextures()[data.NORMAL1SLOT].mtNor=data.NORMAL1DIRECTION 
			# blendMat.getTextures()[data.NORMAL1SLOT].size=data.NORMAL1SIZE 
		##else:
		#	if self.WARNING==True:
		#		print 'failed...',data.normal1	
				
def normal2(blendMat, data): 
		if os.path.exists(data.normal2) == True:
			img = bpy.data.images.load(data.normal2)
			imgName = blendMat.name.replace('-mat-','-norm2-')
			img.name = imgName
			# texname=blendMat.name.replace('-mat-','-norm2-')
			# tex = Blender.Texture.New(texname)
			# tex.setType('Image')
			# tex.image = img 
			# tex.setImageFlags('NormalMap')

			blendMat.use_nodes = True
			tex_node = blendMat.node_tree.nodes.new('ShaderNodeTexImage')
			tex_node.image = img

			normal_map_node = blendMat.node_tree.nodes.new('ShaderNodeNormalMap')
			normal_map_node.inputs['Strength'].default_value = data.NORMAL2STRONG

			mapping_node = blendMat.node_tree.nodes.new('ShaderNodeMapping')
			mapping_node.inputs['Scale'].default_value = data.NORMAL2SIZE

			blendMat.node_tree.links.new(mapping_node.outputs['Vector'], tex_node.inputs['Vector'])
			blendMat.node_tree.links.new(tex_node.outputs['Color'], normal_map_node.inputs['Color'])

			node_principled = next(node for node in blendMat.node_tree.nodes if node.type == 'BSDF_PRINCIPLED')
			blendMat.node_tree.links.new(normal_map_node.outputs['Normal'], node_principled.inputs['Normal'])

			# blendMat.setTexture(data.NORMAL2SLOT,tex,Blender.Texture.TexCo.UV,Blender.Texture.MapTo.NOR)
			# blendMat.getTextures()[data.NORMAL2SLOT].norfac=data.NORMAL2STRONG 
			# blendMat.getTextures()[data.NORMAL2SLOT].mtNor=data.NORMAL2DIRECTION 
			# blendMat.getTextures()[data.NORMAL2SLOT].size=data.NORMAL2SIZE 
		#else:
		#	if self.WARNING==True:
		#		print 'failed...',data.normal2	