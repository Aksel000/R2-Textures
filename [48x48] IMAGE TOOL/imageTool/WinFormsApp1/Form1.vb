
Imports System.Data.SqlClient
Imports System.Security.Cryptography.Xml
Imports System.Windows
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports Microsoft.SqlServer
Imports Microsoft.SqlServer.Management.Smo
Imports Microsoft.SqlServer.Management.Common
Imports Microsoft.SqlServer.Replication
Imports System.Net.Sockets
Imports System.Threading.Tasks
Imports System.IO

Public Class Form1
    Dim connection As New SqlConnection
    Dim selectedFilePath As String
    Private isConnected As Boolean = False


    Private Sub DisplayImage(x As Integer, y As Integer)
        Dim smallImageWidth As Integer = 48 ' 每个小图像的宽度
        Dim smallImageHeight As Integer = 48 ' 每个小图像的高度
        Dim image As New Bitmap(selectedFilePath)
        ItemMap.Image = image

        ' 自动计算每行可以容纳多少个图标
        Dim columns As Integer = CInt(image.Width / smallImageWidth)
        ' 自动计算总共有多少行
        Dim rows As Integer = CInt(image.Height / smallImageHeight)

        Dim sourceX As Integer
        Dim sourceY As Integer

        If x < columns And y < rows Then
            ' 计算选定位置的小图像的矩形区域（使用偏移）
            sourceX = x * smallImageWidth
            sourceY = y * smallImageHeight
        Else
            ' 使用 x 和 y 的原始值，不应用偏移
            sourceX = x
            sourceY = y
        End If

        ' 创建一个矩形
        Dim sourceRect As New Rectangle(sourceX, sourceY, smallImageWidth, smallImageHeight)

        ' 从整个JPEG图像中裁剪出小图像
        Dim smallImage As Bitmap = image.Clone(sourceRect, image.PixelFormat)

        ' 显示小图像在PictureBox1中
        PictureBox1.Image = smallImage
    End Sub



    Private Sub ItemMap_Click(sender As Object, e As EventArgs) Handles ItemMap.Click
        On Error Resume Next
        If itemID.Text = "" Then
            ' SysMsg.Text = "请输入 搜索内容."
            Exit Sub
        End If

        Dim clickPoint As Point = ItemMap.PointToClient(MousePosition)

        ' 计算对应的X和Y坐标
        Dim x As Integer = clickPoint.X \ 48 ' 48是每个小图像的宽度
        Dim y As Integer = clickPoint.Y \ 48 ' 48是每个小图像的高度

        ' 如果 x 大于等于 10，不再除以 48
        'If x >= 10 Then
        TextBoxX.Text = (x * 48).ToString()
        ' Else
        TextBoxY.Text = (y * 48).ToString()
        ' End If

        ' 如果 y 大于等于 10，不再除以 48
        ' If y >= 10 Then
        'TextBoxY.Text = y.ToString()
        ' Else
        ' TextBoxY.Text = (y * 48).ToString()
        ' End If

        ' 然后你可以调用DisplayImage方法来显示相应的图像
        DisplayImage(x, y)
    End Sub


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Panel1.AutoScroll = True
        ItemMap.SizeMode = PictureBoxSizeMode.AutoSize
        ipaddress.Text = My.Settings.IP
        PortTextBox.Text = My.Settings.Port
        User.Text = My.Settings.User
        Pwd.Text = My.Settings.Pwd
        DBNameTxt.Text = My.Settings.DBName
        jpg()
    End Sub

    Public Sub jpg()
        ' 指定要读取的文件夹路径
        Dim folderPath As String = Application.StartupPath + "1702image\" ' 请替换为您的文件夹路径

        ' 检查文件夹是否存在
        If Directory.Exists(folderPath) Then
            ' 获取文件夹中所有的 JPG 文件
            Dim jpgFiles() As String = Directory.GetFiles(folderPath, "*.jpg")

            ' 遍历 JPG 文件并输出文件名（不包括扩展名）
            For Each filePath As String In jpgFiles
                Dim fileNameWithoutExtension As String = Path.GetFileNameWithoutExtension(filePath)
                itemMapName.Items.Add(fileNameWithoutExtension & ".dds")
            Next
        Else
            Console.WriteLine("文件夹不存在：" & folderPath)
        End If
    End Sub



    Private Sub SelectedItem(searchText As String)
        ' 当用户点击按钮时，在 ComboBox 中查找并选择匹配项

        Dim itemToSelect As String = itemMapName.Items.Cast(Of String)().FirstOrDefault(Function(item) item.Contains(searchText))

        If itemToSelect IsNot Nothing Then
            itemMapName.SelectedItem = itemToSelect
        End If

    End Sub







    Private Sub ReadButton_Click(sender As Object, e As EventArgs) Handles ReadButton.Click
        On Error Resume Next
        If itemID.Text = "" Then
            ' SysMsg.Text = "请输入 搜索内容."
            Exit Sub
        End If
        'Dim Conn = New SqlConnection()

        ' Conn.ConnectionString = "Data Source=" & "58.233.132.94" & ";Initial Catalog=FNLParm_CN;Integrated Security=false;User ID=sa;Password=@jlh028058"
        ' Conn.Open()

        Dim Comm As New SqlCommand("select * from dbo." & ComboBox1.Text & "", connection)

        ' Dim ItemsId, ItemsNm, TblName As String
        'If ComboBox1.Text = "DT_ItemResource" Then
        'TblName = "DT_Item"
        'ItemsNm = "IName"
        'ItemsId = "IID"
        'ElseIf ComboBox1.Text = "DT_SkillPack" Then
        'TblName = "DT_SkillPack"
        'ItemsNm = "mName"
        'ItemsId = "mSPID"
        'End If
        'Dim CommName As New SqlCommand("SELECT " & ItemsNm & " FROM dbo." & TblName & " WHERE " & ItemsId & " = " & itemID.Text)
        'Debug.Write(CommName) 





        Comm.ExecuteNonQuery()
        'CommName.ExecuteNonQuery()


        'ListView1.Items.Clear()
        Dim CsvStr As String = itemID.Text

        Dim reader As SqlDataReader = Comm.ExecuteReader()

        'Dim readerNm As SqlDataReader = CommName.ExecuteReader()




        While reader.Read()

            If ComboBox1.Text = "DT_ItemResource" Then
                If reader(1) = CsvStr Then '包括字组串 物品 添加
                    If reader(3) <> "0" AndAlso reader(3) <> "item.dds" Then
                        itemMapName.Text = reader(3)
                        Dim itemImageName As String = reader(3).Replace(".dds", ".jpg")
                        selectedFilePath = Application.StartupPath + "1702image\" + itemImageName
                        Dim image As New Bitmap(selectedFilePath)
                        ItemMap.Image = image
                        ItemName.Text = ItemName.Text

                        TextBoxX.Text = reader(4)
                        TextBoxY.Text = reader(5)
                        DisplayImage(reader(4), reader(5))
                    End If
                End If

            ElseIf ComboBox1.Text = "DT_SkillPack" Then

                If reader(0) = CsvStr Then '包括字组串 物品 添加
                    If reader(11) <> 0 Then
                        ItemName.Text = reader(1)

                        itemMapName.Text = reader(11)
                        Dim itemImageName As String = reader(11).Replace(".dds", ".jpg")
                        selectedFilePath = Application.StartupPath + "1702image\" + itemImageName
                        Dim image As New Bitmap(selectedFilePath)
                        ItemMap.Image = image
                        TextBoxX.Text = reader(12)
                        TextBoxY.Text = reader(13)
                        DisplayImage(reader(12), reader(13))
                    End If
                End If

            End If

        End While
        reader.Close()
        ' Conn.Close()





        Comm.Clone()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim searchText As Integer = Integer.Parse(itemID.Text)

        Dim newTextX As String = TextBoxX.Text
        Dim newTextY As String = TextBoxY.Text

        ' 创建并执行查询语句
        If ComboBox1.Text = "DT_ItemResource" Then
            Dim query As String = "UPDATE " & ComboBox1.Text & " SET RFileName = @RFileName, RPosX = @RPosX, RPosY = @RPosY WHERE ROwnerID = @ROwnerID"
            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@ROwnerID", itemID.Text)
                command.Parameters.AddWithValue("@RFileName", itemMapName.Text)
                command.Parameters.AddWithValue("@RPosX", newTextX)
                command.Parameters.AddWithValue("@RPosY", newTextY)

                command.ExecuteNonQuery()


            End Using
            Command.Clone()
        End If

        If ComboBox1.Text = "DT_SkillPack" Then
            Dim updateQuery As String = "UPDATE DT_SkillPack SET " &
                                        "mSpriteFile = @mSpriteFile, " &
                                        "mSpriteX = @mSpriteX, " &
                                        "mSpriteY = @mSpriteY " &
                                        "WHERE mSPID = @mSPID" ' 根据实际情况替换 YourPrimaryKeyColumn 和 @PrimaryKeyValue

            Using cmd As New SqlCommand(updateQuery, connection)
                cmd.Parameters.AddWithValue("@mSPID", searchText)
                cmd.Parameters.AddWithValue("@mSpriteFile", itemMapName.Text)
                cmd.Parameters.AddWithValue("@mSpriteX", TextBoxX.Text)
                cmd.Parameters.AddWithValue("@mSpriteY", TextBoxY.Text)
                Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
            End Using


        End If
        ' connection.Close()
        ' End Using

    End Sub

    Private Sub ConnectButton_Click(sender As Object, e As EventArgs) Handles ConnectButton.Click
        ConnectToDatabase()
    End Sub

    Private Sub ConnectToDatabase()
        If isConnected Then
            ' 如果已连接，执行断开连接操作
            connection.Close()
            ' 还可以执行其他断开连接后的操作
            ' ...

            ' 更新连接状态和按钮文本
            isConnected = False
            ConnectButton.Text = "Connect"
        Else
            ' 如果未连接，执行连接操作
            ' 显示等待光标或其他UI指示符以指示连接正在进行中
            Me.Cursor = Cursors.WaitCursor
            connection.Close()
            Dim port As Integer = PortTextBox.Text ' SQL Server的默认端口
            Try
                ' 在后台线程中进行数据库连接操作
                Using client As New TcpClient(ipaddress.Text, port)
                    connection.ConnectionString = "Data Source=" & ipaddress.Text & ";Initial Catalog=" & DBNameTxt.Text & ";Integrated Security=false;User ID=" & User.Text & ";Password=" & Pwd.Text
                    connection.Open()
                End Using

                ' 连接成功后的其他操作
                If connection.State = ConnectionState.Open Then
                    ' 连接成功，执行其他UI操作
                    My.Settings.IP = ipaddress.Text
                    My.Settings.Port = PortTextBox.Text
                    My.Settings.User = User.Text
                    My.Settings.Pwd = Pwd.Text
                    My.Settings.DBName = DBNameTxt.Text
                    My.Settings.Save()
                    itemMapName.Enabled = True
                    itemID.Enabled = True
                    ReadButton.Enabled = True
                    TextBoxX.Enabled = True
                    TextBoxY.Enabled = True
                    Button3.Enabled = True
                    ' MessageBox.Show("Connection successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

                ' 更新连接状态和按钮文本
                isConnected = True
                ConnectButton.Text = "disconnect"
            Catch ex As Exception
                ' 处理连接错误
                MessageBox.Show("Failed to connect to the server. Error: " & ex.Message, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                ' 恢复正常光标
                Me.Cursor = Cursors.Default
            End Try
        End If
    End Sub

    Private Sub itemMapName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles itemMapName.SelectedIndexChanged

        Dim selectedItem As String = itemMapName.SelectedItem.ToString()


        Dim itemImageName As String = selectedItem.Replace(".dds", ".jpg")
        selectedFilePath = Application.StartupPath + "1702image\" + itemImageName
        Dim image As New Bitmap(selectedFilePath)
        ItemMap.Image = image

        'DisplayImage(reader(12), reader(13))
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged

    End Sub

    Private Sub GroupBox1_Enter(sender As Object, e As EventArgs) Handles GroupBox1.Enter

    End Sub

    Private Sub Label7_Click(sender As Object, e As EventArgs) Handles Label7.Click

    End Sub

    Private Sub ItemName_TextChanged(sender As Object, e As EventArgs) Handles ItemName.TextChanged

    End Sub
End Class
