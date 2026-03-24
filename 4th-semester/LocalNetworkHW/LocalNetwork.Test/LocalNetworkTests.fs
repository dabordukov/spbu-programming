module LocalNetwork.Tests

open NUnit.Framework
open FsUnit
open Network

type RandomMock(value: double) =
    interface IRandom with
        member r.NextDouble() = value

type OSMock(prob: double) =
    interface IOperatingSystem with
        member o.Name = "TestOS"
        member o.InfectionProbability = prob

[<Test>]
let ``Probability 1 behaves like BFS`` () =
    let os = OSMock 1.0

    let computers =
        [ Computer(1, os, true)
          Computer(2, os, false)
          Computer(3, os, false)
          Computer(4, os, false) ]

    let edges = [ 1, 2; 2, 3; 3, 4 ]

    let random = RandomMock 0.0
    let network = Network(computers, edges, random)

    let network1 = network.Step()
    network1.GetInfectedIds |> should equal [ 1; 2 ]

    let network2 = network1.Step()
    network2.GetInfectedIds |> should equal [ 1; 2; 3 ]

    let network3 = network2.Step()
    network3.GetInfectedIds |> should equal [ 1; 2; 3; 4 ]

[<Test>]
let ``Probability 0 infects no one`` () =
    let os = OSMock 0.0

    let computers =
        [ Computer(1, os, true); Computer(2, os, false); Computer(3, os, false) ]

    let edges = [ 1, 2; 2, 3 ]

    let random = RandomMock 0.5
    let network = Network(computers, edges, random)
    let network1 = network.Step()

    network.Computers |> should equal network1.Computers
    network1.GetInfectedIds |> should equal [ 1 ]

[<Test>]
let ``Virus does not jump over nodes`` () =
    let os = OSMock 1.0

    let computers =
        [ Computer(1, os, true); Computer(2, os, false); Computer(3, os, false) ]

    let edges = [ 1, 2; 2, 3 ]

    let random = RandomMock 0.0
    let network = Network(computers, edges, random)

    let network1 = network.Step()
    network1.GetInfectedIds |> should equal [ 1; 2 ]

    let network2 = network1.Step()
    network2.GetInfectedIds |> should equal [ 1; 2; 3 ]
