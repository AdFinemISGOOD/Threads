Imports System.Threading
Imports System.ComponentModel

Class MainWindow
    Dim DC As New DatCon

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        DC.Proc = 0
        Listenstart()
    End Sub

    Private Sub Button_Click1(sender As Object, e As RoutedEventArgs)
        ChangeVal()
    End Sub

    Private Sub MainWindow_Initialized(sender As Object, e As EventArgs) Handles Me.Initialized
        DataContext = DC
    End Sub

    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Listenstart()
        LoadPG123()
    End Sub

    Private Sub Listenstart()
        Dim thr As New Thread(AddressOf Beeps)
        thr.IsBackground = True
        thr.Start()
    End Sub

    Sub Beeps()
        Dim Torf As Boolean = False
        Do While Torf = False
            If DC.Proc >= DC.NCProc Then
                Beep()
                Torf = True
            Else
                Thread.Sleep(100)
            End If
        Loop
    End Sub

    Private Sub LoadPG123()
        Dim thr As New Thread(AddressOf LoadProccent)
        thr.IsBackground = True
        thr.Start()
    End Sub

    Sub LoadProccent()
        Do Until DC.Proc >= 100
            DC.Proc += 1
            Thread.Sleep(1000)
        Loop
    End Sub
    Private Sub ChangeVal()
        Dim res As String = InputBox("Введите число от 0 до 100", "Значение для звукового сигнала")
        If Not res = "" Then
            If Nummer(res) = True Then
                DC.NCProc = res
                Listenstart()
            End If
        End If
    End Sub

    Private Function Nummer(Value As String) As Boolean
        For Each i In Value
            If Not IsNumeric(i) Then
                Return False
                Exit Function
            End If
        Next i
        Select Case CType(Value, Long)
            Case < 0
                Return False
            Case > 100
                Return False
            Case Else
                Return True
        End Select
    End Function

End Class

Class DatCon
    Implements INotifyPropertyChanged
    Public _NCProc As Long = 100

    Public _Proc As Long = 0
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Public Property NCProc As Long
        Set(value As Long)
            _NCProc = value
            OnPropChang("NCProc")
        End Set
        Get
            Return _NCProc
        End Get
    End Property

    Public Property Proc As Long
        Set(value As Long)
            _Proc = value
            OnPropChang("Proc")
        End Set
        Get
            Return _Proc
        End Get
    End Property

    Public Sub OnPropChang(Optional SenderName As String = "")
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(SenderName))

    End Sub
End Class