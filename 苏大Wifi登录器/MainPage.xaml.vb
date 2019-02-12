' https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板
Imports 苏大Wifi登录器.App, Windows.ApplicationModel.Background
''' <summary>
''' 可用于自身或导航至 Frame 内部的空白页。
''' </summary>
Public NotInheritable Class MainPage
	Inherits Page
	Structure 认证域
		Property 显示名 As String
		Property domain As String
		Sub New(显示名 As String, domain As String)
			Me.显示名 = 显示名
			Me.domain = domain
		End Sub
		Public Overrides Function ToString() As String
			Return 显示名
		End Function
	End Structure
	Private Async Sub 登录_Click(sender As Object, e As RoutedEventArgs) Handles 登录.Click
		Dim b As String = 漫游设置.Values("domain"), a As Task(Of String) = 尝试登录(用户名.Text, 密码.Password, b, 后台任务.IsOn)
		漫游设置.Values(b & "用户名") = 用户名.Text
		漫游设置.Values(b & "密码") = 密码.Password
		响应信息.Text = Await a
	End Sub

	Private Async Sub MainPage_Loaded(sender As FrameworkElement, args As Object) Handles Me.Loaded
		Dim a As Task(Of String) = 检查登录状态()
		认证域ComboBox.ItemsSource = {New 认证域("校园网", ""), New 认证域("中国移动测试", "CMCC"), New 认证域("中国移动", "cmcc-suzhou")}
		认证域ComboBox.SelectedIndex = 0
		For Each b As 认证域 In 认证域ComboBox.Items
			If b.domain = 漫游设置.Values("domain") Then
				认证域ComboBox.SelectedItem = b
				Exit For
			End If
		Next
		认证域ComboBox_SelectionChanged()
		AddHandler 认证域ComboBox.SelectionChanged, AddressOf 认证域ComboBox_SelectionChanged
		响应信息.Text = Await a
		任务通知.IsOn = If(漫游设置.Values("任务通知"), True)
	End Sub

	Private Async Sub 注销_Click(sender As Object, e As RoutedEventArgs) Handles 注销.Click
		响应信息.Text = Await 尝试注销()
	End Sub

	Private Sub 后台任务_Toggled(sender As Object, e As RoutedEventArgs) Handles 后台任务.Toggled
		For Each b As BackgroundTaskRegistration In BackgroundTaskRegistration.AllTasks.Values
			b.Unregister(False)
		Next
		If 后台任务.IsOn Then
			Dim a As New BackgroundTaskBuilder
			a.SetTrigger(New SystemTrigger(SystemTriggerType.NetworkStateChange, False))
			a.AddCondition(New SystemCondition(SystemConditionType.InternetNotAvailable))
			a.Register()
		End If
		漫游设置.Values(漫游设置.Values("domain") & "自动联网") = 后台任务.IsOn
	End Sub

	Private Sub 任务通知_Toggled(sender As Object, e As RoutedEventArgs) Handles 任务通知.Toggled
		漫游设置.Values("任务通知") = 任务通知.IsOn
	End Sub

	Private Sub 认证域ComboBox_SelectionChanged()
		Dim a As String = DirectCast(认证域ComboBox.SelectedItem, 认证域).domain
		With 漫游设置
			.Values("domain") = a
			用户名.Text = If(.Values(a & "用户名"), "")
			密码.Password = If(.Values(a & "密码"), "")
			后台任务.IsOn = If(.Values(a & "自动联网"), False)
		End With
	End Sub

	Private Sub 删除信息_Click(sender As Object, e As RoutedEventArgs) Handles 删除信息.Click
		Dim a As String = 漫游设置.Values("domain")
		With 漫游设置.Values
			.Remove(a & "用户名")
			.Remove(a & "密码")
		End With
		用户名.Text = ""
		密码.Password = ""
	End Sub
End Class
