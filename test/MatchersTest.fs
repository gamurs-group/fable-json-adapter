module Gamurs.Fable.Test.MatchersTest

open Fable.Import.MochaJS
open Gamurs.Fable.Matchers

let private expectMatcherFailure fn =
    let exceptionThrown =
        try
            fn ()
            false
        with
        | _ -> true
    match exceptionThrown with
    | true -> ()
    | false -> failwith "Expected MatcherFailure to be raised."


describe "Matchers" <| fun _ ->

    describe "equalTo" <| fun _ ->

        it "should match if values are equal" <| fun _ ->
            expect 1 <| equalTo 1

        it "should not match if values aren't equal" <| fun _ ->
            expectMatcherFailure <| fun _ ->
                expect 1 <| equalTo 2

    describe "notEqualTo" <| fun _ ->

        it "should match if values are not equal" <| fun _ ->
            expect 1 <| notEqualTo 2

        it "should not match if values are equal" <| fun _ ->
            expectMatcherFailure <| fun _ ->
                expect 1 <| notEqualTo 1

    describe "isError" <| fun _ ->

        it "should match if Result.Error" <| fun _ ->
            expect (Error "an error") <| isError

        it "should not match if Result.Ok" <| fun _ ->
            expectMatcherFailure <| fun _ ->
                expect (Ok "ok") <| isError

    describe "shouldRaiseException" <| fun _ ->

        it "should match if an exception is raised" <| fun _ ->
            expectException <| fun _ ->
                failwith "An expected exception"

        it "should not match if no exception is raised" <| fun _ ->
            expectMatcherFailure <| fun _ ->
                expectException <| fun _ ->
                    ()
