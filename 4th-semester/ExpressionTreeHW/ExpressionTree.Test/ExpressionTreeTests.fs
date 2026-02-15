module ExpressionTree.Test

open NUnit.Framework
open FsUnit
open ExpressionTree

let TestCases =
    [ TestCaseData(Add(Multiply(Add(Number 44, Number 1), Number 5), Multiply(Add(Number 1, Number 1), Number 2)))
          .Returns(Some 229)
      TestCaseData(Divide(Multiply(Add(Number 44, Number 1), Number 5), Add(Add(Number 2, Number 1), Number 2)))
          .Returns(Some 45)
      TestCaseData(Divide(Multiply(Add(Number 44, Number 1), Number 5), Subtract(Add(Number 2, Number 1), Number 8)))
          .Returns(Some -45)
      TestCaseData(Divide(Number 8, Number 0)).Returns(None)
      TestCaseData(Add(Number 8, Divide(Number 8, Number 0))).Returns(None) ]

[<Test; TestCaseSource(nameof (TestCases))>]
let ``Expression should be evaluated correctly`` expr = evaluateExpression expr
