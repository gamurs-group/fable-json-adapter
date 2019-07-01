/// Cross-platform matchers that can be used in both native and fable-compiler tests.
module Gamurs.Fable.Matchers

// TODO This should be pulled out into a separate repository and NuGet package

/// The result of comparing a value against a matcher
type MatcherResult =
    | Success
    | Failure of expected : string
module private MatcherResult =
    let fromBoolean b expected =
        match b with
        | true -> Success
        | false -> Failure expected

/// Raised to indicate a failed match
exception MatcherFailure of string

/// Matchers are used to assert on test results, via <c>expect</c>
type Matcher<'actual> = 'actual -> MatcherResult

let private failedMatch expected actual : unit =
    let description = failwithf "Expected:\n\n%s\n\nActual:\n\n%s" expected actual
    raise (MatcherFailure description) |> ignore

/// <summary>
///     Assert using a matcher
/// </summary>
///
/// <param name="actual">
///     The actual value to which the matcher should be applied.
/// </param>
///
/// <param name="matcher">
///     The matcher to apply.
/// </param>
let expect (actual : 'actual) (matcher : Matcher<'actual>) : unit =
    let result = matcher actual
    match result with
    // Test case success indicated by unit return value
    | Success ->
        ()
    // Fail test case
    | Failure expected ->
        let actualStr = sprintf "%A" actual
        failedMatch expected actualStr


/// Usage: expect 1 <| equalTo 1
let equalTo (expected : 'a) (actual : 'a) : MatcherResult =
    let expectedStr = sprintf "%A" expected
    MatcherResult.fromBoolean (actual = expected) expectedStr

/// Usage: expect 1 <| notEqualTo 1
let notEqualTo (expected : 'a) (actual : 'a) : MatcherResult =
    let expectedStr = sprintf "%A" expected
    MatcherResult.fromBoolean (actual <> expected) expectedStr

/// Usage: expect actual <| isSome
let isSome (actual : 'a option) : MatcherResult =
    match actual with
    | Some _ -> Success
    | None -> Failure "Option.None"

/// Usage: expect actual <| isNone
let isNone (actual : 'a option) : MatcherResult =
    match actual with
    | None -> Success
    | Some _ -> Failure "Option.Some"

/// Usage: expect actual <| isError
let isError (actual : Result<_,_>) : MatcherResult =
    match actual with
    | Error _ -> Success
    | _ -> Failure "Result.Error"


/// <summary>
///     Assert that the provided function raises an exception.
/// </summary>
/// <param name="fn">
///     The function, which should raise an exception when executed.
/// </param>
let expectException (fn : unit -> 'a) : unit =
    let result =
        try
            fn() |> ignore
            Failure "Exception to be raised"
        with
        | ex ->
            Success

    // Report result
    match result with
    | Success ->
        ()
    // Fail test case
    | Failure expected ->
        failedMatch expected "No exception was raised"
