' https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板
Imports 苏大Wifi登录器.App, Windows.ApplicationModel.Background
''' <summary>
''' 可用于自身或导航至 Frame 内部的空白页。
''' </summary>
Public NotInheritable Class MainPage
	Inherits Page
	Private Async Sub 登录_Click(sender As Object, e As RoutedEventArgs) Handles 登录.Click
		Dim a As Task(Of String) = 尝试登录(用户名.Text, 密码.Password)
		漫游设置.Values("用户名") = 用户名.Text
		漫游设置.Values("密码") = 密码.Password
		响应信息.Text = Await a
	End Sub

	Private Async Sub Wifi检测()
		Dim a As Wlan现状, c As New Text.StringBuilder("当前已连接：
网络名" & vbTab & "信号"), d As Boolean = False
		a = Await Wlan信息()
		For Each b As KeyValuePair(Of String, Byte) In a.当前连接
			c.Append("
").Append(b.Key).Append(vbTab).Append(b.Value)
			If b.Key.Contains("SUDA_WIFI") Then d = True
		Next
		c.Append("
找到的苏大Wifi：")
		For Each b As KeyValuePair(Of String, Byte) In a.可用连接
			c.Append("
").Append(b.Key).Append(vbTab).Append(b.Value)
		Next
		If d = False Then c.Append("
当前未连接到苏大Wifi。")
		If a.可用连接.Any = False Then c.Append("没有找到可用的苏大Wifi，或信号太弱。尝试让你的设备靠近信号源。")
		If d = False AndAlso a.可用连接.Any Then c.Append("发现可用的苏大Wifi，请尝试连接。")
		Wifi信息.Text = c.ToString
	End Sub

	Private Async Sub MainPage_Loading(sender As FrameworkElement, args As Object) Handles Me.Loading
		Dim a As Task(Of String) = 检查登录状态()
		用户名.Text = If(漫游设置.Values("用户名"), "")
		密码.Password = If(漫游设置.Values("密码"), "")
		后台任务.IsOn = If(漫游设置.Values("自动联网"), False)
		任务通知.IsOn = If(漫游设置.Values("任务通知"), True)
		Wifi检测()
		响应信息.Text = Await a
	End Sub

	Private Async Sub 注销_Click(sender As Object, e As RoutedEventArgs) Handles 注销.Click
		响应信息.Text = Await 尝试注销()
	End Sub

	Private Sub 刷新_Click(sender As Object, e As RoutedEventArgs) Handles 刷新.Click
		Wifi检测()
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
		漫游设置.Values("自动联网") = 后台任务.IsOn
	End Sub

	Private Sub 任务通知_Toggled(sender As Object, e As RoutedEventArgs) Handles 任务通知.Toggled
		漫游设置.Values("任务通知") = 任务通知.IsOn
	End Sub
End Class
