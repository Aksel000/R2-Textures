from .functionsLib import *
from mathutils import *
import bpy


class Bone:
	def __init__(self):
		self.ID = None
		self.name = None
		self.parentID = None
		self.parentName = None
		self.quat = None
		self.pos = None
		self.matrix = None
		self.posMatrix = None
		self.rotMatrix = None
		self.scaleMatrix = None
		self.children = []
		self.edit = None	


class Skeleton:
	def __init__(self):
		self.name = 'armature'
		self.arm_name = 'armature'
		self.boneList = []
		self.armature = None  
		self.object = None
		self.boneNameList = [] 
		self.ARMATURESPACE = False
		self.BONESPACE = False
		self.DEL = True
		self.NICE = False
		self.IK = False
		self.BINDMESH = False
		self.WARNING = False
		self.SORT = False
		self.matrix = None
		
	def log(self, message, type='INFO'):
		if self.WARNING == True:
			print(f'{type}: {message}')
		
	def boneChildren(self, parentBlenderBone, parentBone):
		for child in parentBlenderBone.children:
			for bone in self.boneList:
				if bone.name == child.name:
					blenderBone = self.armature.bones[bone.name]
					bone.matrix = bone.matrix @ parentBone.matrix
					self.boneChildren(blenderBone, bone)
		
	def createChildList(self):
		for boneID in range(len(self.boneList)):
			bone = self.boneList[boneID]
			name = bone.name
			blenderBone = self.armature.bones[name]
			if blenderBone.parent is None:
				self.boneChildren(blenderBone, bone)

	def draw(self): 
		self.log(f'Skeleton.name: {self.name}')
		self.log(f'Skeleton.boneList: {len(self.boneList)}')
		self.log(f'Skeleton.ARMATURESPACE: {self.ARMATURESPACE}')
		self.log(f'Skeleton.BONESPACE: {self.BONESPACE}')
			
		self.check()

		if len(self.boneList) > 0:
			self.create_bones()
			self.create_bone_connection()

			if self.SORT == True:
				self.createChildList()

			self.create_bone_position()			

		if self.BINDMESH is True:
			scene = bpy.context.scene
			for object in scene.objects:
				if object.type == 'Mesh':
					object.parent = self.object
					# TODO ?
					# object.modifiers.new('Armature', 'ARMATURE').object = self.object
					object.parent_type = 'OBJECT'
					object.use_deform = True

		if self.IK == True:
			self.armature.display_type = 'OCTAHEDRAL'
			for key in self.armature.bones.keys():
				bone = self.armature.bones[key]
				children = bone.children

				if len(children) == 1:
					bpy.context.view_layer.objects.active = self.object
					bpy.ops.object.mode_set(mode='EDIT')

					ebone = self.armature.edit_bones[bone.name]
					if ebone.tail != children[0].head['ARMATURESPACE']:
						ebone.tail = children[0].head['ARMATURESPACE']

					bpy.ops.object.mode_set(mode='OBJECT')

			for key in self.armature.bones.keys():
				bone = self.armature.bones[key]
				children = bone.children

				if len(children) == 1:
					bpy.context.view_layer.objects.active = self.object
					bpy.ops.object.mode_set(mode='EDIT')

					self.armature.edit_bones[children[0].name].use_connect = True

					bpy.ops.object.mode_set(mode='OBJECT')

			if self.IK == True:
				self.object.pose.use_auto_ik = True

	def create_bones(self): 
		bpy.context.view_layer.objects.active = self.object
		bpy.ops.object.mode_set(mode='EDIT')
		boneList = []

		for bone in self.armature.bones.values():
			if bone.name not in boneList:
				boneList.append(bone.name)
		
		for boneID in range(len(self.boneList)):
			name = self.boneList[boneID].name
			
			if name is None:
				name = str(boneID)
				self.boneList[boneID].name = name

			self.boneNameList.append(name)
			if name not in boneList:
				bone = self.armature.edit_bones.new(name)
				bone.tail = (1,0,0) # hack to save bone after exit from edit mode

		bpy.ops.object.mode_set(mode='OBJECT')

	def create_bone_connection(self):
		bpy.context.view_layer.objects.active = self.object
		bpy.ops.object.mode_set(mode='EDIT')
		
		for boneID in range(len(self.boneList)):
			name = self.boneList[boneID].name
			if name is None:
				name = str(boneID)

			bone = self.armature.edit_bones.get(name)
			parentID = None
			parentName = None
			
			if self.boneList[boneID].parentID is not None:
				parentID = self.boneList[boneID].parentID
				if parentID != -1:
					parentName = self.boneList[parentID].name
			
			if self.boneList[boneID].parentName is not None:
				parentName = self.boneList[boneID].parentName
			
			if parentName is not None:  
				parent = self.armature.edit_bones.get(parentName)
				if parent:
					if parentID is not None:
						if parentID != -1:
							bone.parent = parent
					else:
						bone.parent = parent
			else:
				self.log(f'No parent for bone {name}', 'WARNING')

		bpy.ops.object.mode_set(mode='OBJECT')
		
	def create_bone_position(self):
		bpy.context.view_layer.objects.active = self.object
		bpy.ops.object.mode_set(mode='EDIT')

		# set bones position
		for m in range(len(self.boneList)):
			name = self.boneList[m].name
			rotMatrix = self.boneList[m].rotMatrix
			posMatrix = self.boneList[m].posMatrix
			scaleMatrix = self.boneList[m].scaleMatrix
			matrix = self.boneList[m].matrix
			bone = self.armature.edit_bones.get(name)
			if bone is None:
				break

			if matrix is not None:
				if self.ARMATURESPACE == True:
					bone.matrix = matrix	
							
					if self.NICE == True:
						bvec = bone.tail - bone.head
						bvec.normalize()
						bone.tail = bone.head + 0.01 * bvec
				elif self.BONESPACE == True:
					rotMatrix = matrix.to_quaternion().to_matrix().to_4x4()
					posMatrix = matrix.to_translation()
					
					if bone.parent:
						bone.head = posMatrix @ bone.parent.matrix + bone.parent.head
						tempM = rotMatrix @ bone.parent.matrix 
						bone.matrix = tempM
					else:
						bone.head = posMatrix
						bone.matrix = rotMatrix
					
					if self.NICE == True:
						bvec = bone.tail - bone.head
						bvec.normalize()
						bone.tail = bone.head + 0.01 * bvec 
				else:
					self.log('ARMATUREPACE or BONESPACE ?', 'ERROR')
			elif rotMatrix is not None and posMatrix is not None:
				if self.ARMATURESPACE == True:
					rotMatrix = roundMatrix(rotMatrix, 4)
					posMatrix = roundMatrix(posMatrix, 4)
					bone.matrix = rotMatrix @ posMatrix

					if self.NICE == True:
						bvec = bone.tail - bone.head
						bvec.normalize()
						bone.tail = bone.head + 0.01 * bvec
				elif self.BONESPACE == True:
					rotMatrix = roundMatrix(rotMatrix, 4).to_matrix().to_4x4()
					posMatrix = roundMatrix(posMatrix, 4).to_translation()
					
					if bone.parent:
						bone.head = posMatrix @ bone.parent.matrix + bone.parent.head
						tempM = rotMatrix @ bone.parent.matrix 
						bone.matrix = tempM
					else:
						bone.head = posMatrix
						bone.matrix = rotMatrix

					if self.NICE == True:
						bvec = bone.tail - bone.head
						bvec.normalize()
						bone.tail = bone.head + 0.01 * bvec 
				else:
					self.log('ARMATUREPACE or BONESPACE ?', 'ERROR')
			else:
				self.log('rotMatrix or posMatrix or matrix is None', 'WARNINIG')
							
		bpy.ops.object.mode_set(mode='OBJECT')

	def check(self):
		scene = bpy.context.scene
		for object in scene.objects:
			if object.type == 'Armature' and object.name == self.arm_name:
				bpy.context.collection.objects.unlink(object)

		for object in bpy.data.objects:
			if object.name == self.name:
				self.object = bpy.data.objects[self.name]
				self.armature = self.object.data

				if self.DEL == True:  
					bpy.context.view_layer.objects.active = self.object
					bpy.ops.object.mode_set(mode='EDIT')

					for bone in self.armature.edit_bones.values():
						del self.armature.edit_bones[bone.name]

					bpy.ops.object.mode_set(mode='OBJECT')
		
		if self.armature == None: 
			self.armature = bpy.data.armatures.new(self.arm_name)

		if self.object == None: 
			self.object = bpy.data.objects.new(self.name, self.armature)

		bpy.context.collection.objects.link(self.object)

		self.armature.display_type = 'STICK'
		self.object.show_in_front = True
		self.matrix = self.object.matrix_world.copy()