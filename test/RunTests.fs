module Gamurs.Fable.Test.RunTests

open Fable.Core.JsInterop

// All tests to be run via Mocha MUST be imported here
importSideEffects "./MatchersTest.fs"
importSideEffects "./JsonAdapterTest.fs"
