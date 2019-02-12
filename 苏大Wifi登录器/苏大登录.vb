Imports System.Net.Http, System.Text
Module 苏大登录

	ReadOnly HTTP客户端 As HttpClient = New HttpClient, 请求正文 As New Dictionary(Of String, String)

	Private Async Function 提取响应信息(请求任务 As Task(Of HttpResponseMessage)) As Task(Of String)
		Try
			Return Newtonsoft.Json.Linq.JObject.Parse(RegularExpressions.Regex.Unescape(Await (Await 请求任务).Content.ReadAsStringAsync))("info")
		Catch ex As HttpRequestException
			Return "未能连接到a.suda.edu.cn，请检查你的网络配置"
		End Try
	End Function

	Async Function 尝试登录(用户名 As String, 密码 As String, domain As String, Optional MAC免认证 As Boolean = True) As Task(Of String)
		请求正文("password") = Convert.ToBase64String(Encoding.Default.GetBytes(密码))
		请求正文("username") = 用户名
		请求正文("enablemacauth") = If(MAC免认证, "1", "0")
		请求正文("domain") = domain
		Return Await 提取响应信息(HTTP客户端.PostAsync("http://a.suda.edu.cn/index.php/index/login", New FormUrlEncodedContent(请求正文)))
	End Function

	Async Function 检查登录状态() As Task(Of String)
		Return Await 提取响应信息(HTTP客户端.GetAsync("http://a.suda.edu.cn/index.php/index/init?_=1520126647184", New HttpCompletionOption))
	End Function

	Async Function 尝试注销() As Task(Of String)
		Return Await 提取响应信息(HTTP客户端.PostAsync("http://a.suda.edu.cn/index.php/index/logout", Nothing))
	End Function
End Module
