Imports Windows.Networking.Connectivity， Windows.Devices.WiFi
Module Wifi连接
	Class Wlan现状
		Property 当前连接 As New Dictionary(Of String, Byte)
		Property 可用连接 As New Dictionary(Of String, Byte)
	End Class

	Async Function Wlan信息() As Task(Of Wlan现状)
		Dim c As New Wlan现状, d As ConnectionProfile
		For Each a As WiFiAdapter In Await WiFiAdapter.FindAllAdaptersAsync
			For Each b As WiFiAvailableNetwork In a.NetworkReport.AvailableNetworks
				If b.Ssid.Contains("SUDA") Then c.可用连接(b.Ssid) = b.SignalBars
			Next
			d = Await a.NetworkAdapter.GetConnectedProfileAsync
			If d IsNot Nothing AndAlso d.WlanConnectionProfileDetails IsNot Nothing Then c.当前连接(d.WlanConnectionProfileDetails.GetConnectedSsid()) = d.GetSignalBars
		Next
		Return c
	End Function

	Async Function 已连接苏大Wifi() As Task(Of Boolean)
		Dim d As ConnectionProfile
		For Each a As WiFiAdapter In Await WiFiAdapter.FindAllAdaptersAsync
			d = Await a.NetworkAdapter.GetConnectedProfileAsync
			If d IsNot Nothing AndAlso d.WlanConnectionProfileDetails.GetConnectedSsid().Contains("SUDA") Then Return True
		Next
		Return False
	End Function

End Module
