import bpy


class ActionBone:
	def	__init__(self):
		self.name = None
		self.posFrameList = []
		self.rotFrameList = []
		self.scaleFrameList = []
		self.posKeyList = []
		self.rotKeyList = []
		self.scaleKeyList = []
		self.matrixFrameList = []
		self.matrixKeyList = []
		
	
class Action:
	def __init__(self):
		self.frameCount = None
		self.name = 'action'
		self.skeleton = 'armature'
		self.boneList = []
		self.ARMATURESPACE = False
		self.BONESPACE = False
		self.FRAMESORT = False
		self.BONESORT = False
		
	def boneNameList(self):
		if self.skeleton is not None:
			scene = bpy.context.scene
			for object in scene.objects:
				if object.name == self.skeleton:
					self.boneNameList = object.data.bones.keys()
		
	def setContext(self):
		scene = bpy.context.scene
		if self.frameCount is not None:
			scene.frame_set(self.frameCount)	
		
	def draw(self):
		scene = bpy.context.scene
		skeleton = None
		
		if self.skeleton is not None:
			for object in scene.objects:
				if object.type == 'ARMATURE':
					if object.name == self.skeleton:				
						skeleton = object
		else:
			print('WARNING: no armature')

		if skeleton is not None:			
			armature = skeleton.data
			pose = skeleton.pose
			action = bpy.data.actions.new(self.name)

			# TODO ?			
			if skeleton.animation_data is None:
				skeleton.animation_data_create()

			skeleton.animation_data.action = action
			timeList=[]
			
			if self.FRAMESORT is True:
				frameList=[]

				for m in range(len(self.boneList)):
					actionbone = self.boneList[m]

					for n in range(len(actionbone.posFrameList)):
						frame = actionbone.posFrameList[n]
						if frame not in frameList:
							frameList.append(frame)
					for n in range(len(actionbone.rotFrameList)):
						frame = actionbone.rotFrameList[n]
						if frame not in frameList:
							frameList.append(frame)
					for n in range(len(actionbone.matrixFrameList)):
						frame = actionbone.matrixFrameList[n]
						if frame not in frameList:
							frameList.append(frame)
										
				for k in range(len(frameList)):
					frame = sorted(frameList)[k]
					for m in range(len(self.boneList)):
						actionbone = self.boneList[m]
						name = actionbone.name
						pbone = pose.bones[name]

						if pbone is not None:
							for n in range(len(actionbone.posFrameList)):
								if frame == actionbone.posFrameList[n]:
									timeList.append(frame)
									poskey = actionbone.posKeyList[n]
									bonematrix = poskey
									
									if self.ARMATURESPACE is True:
										pbone.matrix = bonematrix
										pbone.keyframe_insert(data_path='matrix', frame=frame)
										# pose.update() TODO: check if this is needed
									
									if self.BONESPACE is True:
										if pbone.parent:		
											pbone.matrix = bonematrix @ pbone.parent.matrix
										else:
											pbone.matrix = bonematrix

										pbone.keyframe_insert(data_path='matrix', frame=frame)
										#pose.update()
										
							for n in range(len(actionbone.rotFrameList)):
								if frame == actionbone.rotFrameList[n]:
									timeList.append(frame)
									rotkey = actionbone.rotKeyList[n]
									bonematrix = rotkey

									if self.ARMATURESPACE is True:
										pbone.matrix = bonematrix
										pbone.keyframe_insert(data_path='matrix', frame=frame)
										# pose.update()

									if self.BONESPACE is True:
										if pbone.parent:		
											pbone.matrix = bonematrix @ pbone.parent.matrix
										else:
											pbone.matrix = bonematrix

										pbone.keyframe_insert(data_path='matrix', frame=frame)
											
							for n in range(len(actionbone.matrixFrameList)):
								if frame == actionbone.matrixFrameList[n]:
									timeList.append(frame)
									matrix = actionbone.matrixKeyList[n]
									
									if self.ARMATURESPACE is True:
										pbone.matrix = matrix
										pbone.keyframe_insert(data_path='matrix', frame=1+frame)
									
									if self.BONESPACE is True:
										if pbone.parent:		
											pbone.matrix = matrix @ pbone.parent.matrix
										else:
											pbone.matrix = skeleton.matrix_world @ matrix

										pbone.keyframe_insert(data_path='matrix', frame=1+frame)
					#pose.update()
			elif self.BONESORT is True:
				for m in range(len(self.boneList)):
					actionbone = self.boneList[m]
					name = actionbone.name
					pbone = pose.bones[name]
					
					if pbone is not None:
						# pbone.insertKey(skeleton,0,[Blender.Object.Pose.ROT,Blender.Object.Pose.LOC],True)
						# pose.update()
						# TODO: fix this
						pbone.keyframe_insert(data_path='matrix', frame=0)
						
						for n in range(len(actionbone.posFrameList)):
							frame = actionbone.posFrameList[n]
							timeList.append(frame)
							poskey = actionbone.posKeyList[n]
							bonematrix = poskey
							
							if self.ARMATURESPACE is True:
								pbone.matrix = bonematrix
								pbone.keyframe_insert(data_path='matrix', frame=1+frame)
								#pose.update()
							
							if self.BONESPACE is True:
								if pbone.parent:		
									pbone.matrix = bonematrix @ pbone.parent.matrix
								else:
									pbone.matrix = bonematrix
								
								pbone.keyframe_insert(data_path='matrix', frame=1+frame)
								#pose.update()
								
						for n in range(len(actionbone.rotFrameList)):
							frame = actionbone.rotFrameList[n]
							timeList.append(frame)
							rotkey = actionbone.rotKeyList[n]
							bonematrix = rotkey
							
							if self.ARMATURESPACE is True:
								pbone.matrix = bonematrix
								pbone.keyframe_insert(data_path='matrix', frame=1+frame)
								#pose.update()

							if self.BONESPACE is True:
								print(f'[{n}] matrix: {bonematrix.to_euler()}')
								if pbone.parent:		
									pbone.matrix = bonematrix @ pbone.parent.matrix
								else:
									pbone.matrix = bonematrix

								pbone.keyframe_insert(data_path='matrix', frame=1+frame)
								#pose.update()
								
						for n in range(len(actionbone.matrixFrameList)):
							frame = actionbone.matrixFrameList[n]
							timeList.append(frame)
							matrixkey = actionbone.matrixKeyList[n]
							bonematrix = matrixkey
							
							if self.ARMATURESPACE is True:
								pbone.matrix = skeleton.matrix_world @ bonematrix
								pbone.keyframe_insert(data_path='matrix', frame=1+frame)
								#pose.update()
							
							if self.BONESPACE is True:
								if pbone.parent:		
									pbone.matrix = bonematrix @ pbone.parent.matrix
								else:
									pbone.matrix = bonematrix
								
								pbone.keyframe_insert(data_path='matrix', frame=1+frame)
						#pose.update()
			else:
				print('WARNING: missing BONESORT or FRAMESORT')

			if len(timeList) > 0:	
				self.frameCount = max(timeList)
		else:
			print('WARNING: no skeleton')