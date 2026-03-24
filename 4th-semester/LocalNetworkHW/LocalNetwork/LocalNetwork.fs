namespace Network

open System

/// Represents a source of random values used during infection attempts.
type IRandom =
    /// Returns the next pseudo-random value in the range [0.0; 1.0).
    abstract member NextDouble: unit -> double

/// Default implementation of <see cref="IRandom" /> backed by <see cref="System.Random" />.
type DefaultRandom() =
    let random = Random()

    interface IRandom with
        member r.NextDouble() = random.NextDouble()


/// Describes an operating system and its resistance to infection.
type IOperatingSystem =
    /// Gets the operating system name.
    abstract member Name: string

    /// Gets the probability of infection for a single attempt.
    abstract member InfectionProbability: double

/// Base implementation of <see cref="IOperatingSystem" />.
/// <param name="name">Operating system display name.</param>
/// <param name="infectionProbability">Probability of infection for a single attempt.</param>
type BaseOS(name: string, infectionProbability: double) =
    let name = name
    let infectionProbability = infectionProbability

    interface IOperatingSystem with
        member o.Name = name
        member o.InfectionProbability = infectionProbability

/// Linux operating system.
type LinuxOS() =
    inherit BaseOS("Linux", 0.3)

/// macOS operating system.
type MacOS() =
    inherit BaseOS("MacOS", 0.4)

/// Windows operating system.
type WindowsOS() =
    inherit BaseOS("Windows", 0.8)

/// Represents a computer in the simulated network.
/// <param name="id">Unique computer identifier.</param>
/// <param name="os">Operating system installed on the computer.</param>
/// <param name="infected">Initial infection state.</param>
type Computer(id, os: IOperatingSystem, infected) =
    /// Gets the computer identifier.
    member c.Id = id

    /// Gets the operating system installed on the computer.
    member c.OS = os

    /// Gets a value indicating whether the computer is currently infected.
    member c.IsInfected = infected

    /// Gets a value indicating whether the computer can still become infected.
    member c.CanBeInfected = not infected && os.InfectionProbability > 0.0

    /// Returns a copy of the computer marked as infected.
    member c.Infect() = Computer(id, os, true)

    override c.ToString() =
        let status = if infected then "Infected" else "Not Infected"

        sprintf "Computer %d | %s | %s" id os.Name status

/// Represents a network of connected computers and simulates virus spreading.
/// <param name="computers">Computers that participate in the simulation.</param>
/// <param name="edges">Undirected connections between computer identifiers.</param>
/// <param name="random">Random value provider used for infection attempts.</param>
type Network(computers: Computer list, edges, random: IRandom) =

    let neighbors id =
        edges
        |> List.collect (fun (a, b) ->
            if a = id then [ b ]
            elif b = id then [ a ]
            else [])

    let isAdjacentToInfected infectedIds (computer: Computer) =
        neighbors computer.Id |> List.exists (fun id -> List.contains id infectedIds)

    /// Gets identifiers of all currently infected computers.
    member n.GetInfectedIds =
        computers |> List.filter (fun c -> c.IsInfected) |> List.map (fun c -> c.Id)

    /// Gets the current infection state for each computer as identifier/state pairs.
    member n.Computers = computers |> List.map (fun c -> c.Id, c.IsInfected)

    /// Prints the current network state for the specified simulation step.
    /// <param name="step">Current simulation step number.</param>
    member n.Print(step: int) =
        printfn "Step %d" step
        computers |> List.iter (fun c -> printfn "%O" c)
        printfn ""

    /// Determines whether another simulation step can change the network state.
    member n.CanChange() =
        let infectedIds = n.GetInfectedIds

        computers
        |> List.exists (fun c -> c.CanBeInfected && isAdjacentToInfected infectedIds c)

    /// Produces the next network state after one infection step.
    member n.Step() =
        let infectedIds = n.GetInfectedIds

        let newComputers =
            computers
            |> List.map (fun c ->
                if c.IsInfected then
                    c
                elif isAdjacentToInfected infectedIds c then
                    let roll = random.NextDouble()

                    if roll < c.OS.InfectionProbability then c.Infect() else c
                else
                    c)

        Network(newComputers, edges, random)

    /// Runs the simulation and prints the network state after each step.
    member n.Run() =
        let rec loop step (current: Network) =
            current.Print(step)

            if current.CanChange() then
                let next = current.Step()
                loop (step + 1) next
            else
                ()

        loop 0 n
