open Network

[<EntryPoint>]
let main _ =
    let windows = WindowsOS()
    let linux = LinuxOS()
    let mac = MacOS()

    let computers =
        [ Computer(1, windows, true)
          Computer(2, linux, false)
          Computer(3, windows, false)
          Computer(4, mac, false)
          Computer(5, linux, false) ]

    let edges = [ 1, 2; 2, 3; 2, 4; 4, 5 ]

    let random = DefaultRandom() :> IRandom
    let network = Network(computers, edges, random)

    network.Run()
    0
