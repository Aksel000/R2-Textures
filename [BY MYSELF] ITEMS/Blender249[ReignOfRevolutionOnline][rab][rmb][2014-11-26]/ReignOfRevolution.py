import newGameLib
from newGameLib import *
import Blender

def rmbParser(filename,g):
	#g.logOpen()
	#g.debug=True
	g.seek(0x14) #seek_set
	texture_count = g.i(1)[0]
	mesh_count = g.i(1)[0]
	bone_count = g.i(1)[0]
	mesh_offset = g.i(1)[0]
	texDir=g.dirname.split('model')[0]+'texture'
	if os.path.exists(texDir)==False:texDir=g.dirname
	texList=[]
	for a in range(texture_count):
		t=g.tell()
		texname = g.find('\x00')
		texList.append(texDir+os.sep+texname)
		g.seek(t+260)
	meshList=[]	
	for a in range(mesh_count):
		mesh=Mesh()	
		pos=g.tell()
		g.seek(pos+8)
		mesh.name=g.find('\x00')
		g.seek(pos+72)
		mesh.parentBone=g.find('\x00')
		g.seek(pos+34*4)
		mesh.info=g.i(5)
		g.seek(pos+2156)
		meshList.append(mesh)
	
	
	
	skeleton=Skeleton()
	skeleton.ARMATURESPACE=True
	#skeleton.BONESPACE=True
	skeleton.NICE=True
	for a in range(bone_count):# do (
		bone=Bone()
		bone.ID = g.i(1)[0]
		bone.parentID = g.i(1)[0]
		g.seek(84,1) #seek_cur
		bone.name = g.word(0x40)
		parentName = g.word(0x40)
		Matrix4x4(g.f(16))
		Matrix4x4(g.f(16))
		bone.matrix=Matrix4x4(g.f(16)).invert()
		skeleton.boneList.append(bone)
	skeleton.draw()	
		
	for mesh in meshList:
		print mesh.info
		boneMap=g.B(mesh.info[2])
		for b in range(mesh.info[3]):mesh.vertPosList.append(g.f(3))
		g.seek(mesh.info[3]*12,1)
		for b in range(mesh.info[3]):mesh.vertUVList.append(g.f(2))
		g.seek(mesh.info[3]*12,1)
		g.seek(mesh.info[3]*12,1)
		if mesh.info[0]!=0:
			for b in range(mesh.info[3]):mesh.skinWeightList.append(g.f(4))
			for b in range(mesh.info[3]):mesh.skinIndiceList.append(g.B(4))
			skin=Skin()
			skin.boneMap=boneMap
			mesh.skinList.append(skin)
			mesh.boneNameList=skeleton.boneNameList
		else:
			for b in range(mesh.info[3]):mesh.skinWeightList.append([1.0])
			for b in range(mesh.info[3]):mesh.skinIndiceList.append([0])
			skin=Skin()
			skin.boneMap=[0]
			mesh.skinList.append(skin)
			mesh.boneNameList=[mesh.parentBone]
			
		
		mesh.indiceList=g.H(mesh.info[4])
		mesh.TRIANGLE=True
		mesh.BINDSKELETON='armature'
		mat=Mat()
		mat.TRIANGLE=True
		mat.diffuse=texList[mesh.info[1]]
		mesh.matList.append(mat)
		for bone in skeleton.boneList:
			if bone.name==mesh.parentBone:					
				mesh.matrix=skeleton.object.getMatrix()*bone.matrix
		mesh.draw()
	
	
	#g.debug=True
	#g.tell()
	#g.logClose()
	
def rabParser(filename,g):
	#g.logOpen()
	action=Action()
	#action.ARMATURESPACE=True
	action.BONESPACE=True
	action.BONESORT=True
	action.skeleton='armature'
	X=g.i(9)
	A,B,C=[],[],[]
	for i in range(X[7]):
		A.append(g.word(64))
		B.append(g.i(1)[0])
		C.append(g.i(1)[0])
	g.seek(X[8])	
	g.tell()
	for i in range(X[7]):
		bone=ActionBone()
		bone.name=A[i]
		D=g.i(C[i])		
		E=g.i(B[i])
		for j in range(C[i]):
			bone.posFrameList.append(D[j]//160)
			bone.posKeyList.append(VectorMatrix(g.f(3)))
		for j in range(B[i]):
			bone.rotFrameList.append(E[j]//160)
			matrix=QuatMatrix(g.f(4)).resize4x4().invert()
			if j==0:
				bone.rotKeyList.append(matrix)
			else:
				matrix0=bone.rotKeyList[j-1]*matrix
				bone.rotKeyList.append(matrix0)
		action.boneList.append(bone)
	action.draw()
	action.setContext()	
	#g.debug=True
	#g.tell()	
	#g.logClose()
	
def posortujPliki(dir):	
	for file in os.listdir(os.path.dirname(filename)):
		filePath=os.path.dirname(filename)+os.sep+file
		ext=filePath.split('.')[-1].lower()	
		if ext=='rmb' and '_' not in file:
			sys=Sys(filePath)
			sys.addDir(sys.base)
			shutil.move(filePath,sys.dir+os.sep+sys.base+os.sep+file)
			for afile in os.listdir(os.path.dirname(filename)):
				afilePath=os.path.dirname(filename)+os.sep+afile
				if sys.base+'_' in afile:
					shutil.move(afilePath,sys.dir+os.sep+sys.base+os.sep+afile)
	
def Parser():	
	filename=input.filename
	ext=filename.split('.')[-1].lower()	
	
				
	
	if ext=='rmb':
		file=open(filename,'rb')
		g=BinaryReader(file)			
		rmbParser(filename,g)
		file.close()
	
	if ext=='rab':
		file=open(filename,'rb')
		g=BinaryReader(file)
		rabParser(filename,g)
		file.close()
 
def openFile(flagList):
	global input,output
	input=Input(flagList)
	output=Output(flagList)
	parser=Parser()
	

Blender.Window.FileSelector(openFile,'import','rmb - skinned mesh, rab - animation') 	