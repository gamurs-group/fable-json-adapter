module Gamurs.Fable.Test.JsonAdapterTest

open System
open Fable.Core
open Fable.Core.JsInterop
open Fable.Import.MochaJS
open Gamurs.Fable.Matchers
open Gamurs.Fable.JsonAdapter

[<Import("default", from="./JsonAdapterFixture.json")>]
let exampleJson : obj = jsNative

describe "JsonAdapter" <| fun _ ->

    describe "getObjectOption" <| fun _ ->

        it "should parse an object field as Some" <| fun _ ->
            // Act
            let actual = JsonAdapter.getObjOption "obj" exampleJson

            // Assert
            expect actual <| isSome
            expect actual?field <| equalTo "value"

        it "should parse an empty object field as Some" <| fun _ ->
            // Act
            let actual = JsonAdapter.getObjOption "emptyObj" exampleJson

            // Assert
            expect actual <| isSome

        it "should parse a missing field as None" <| fun _ ->
            // Act
            let actual = JsonAdapter.getObjOption "missing" exampleJson

            // Assert
            expect actual <| isNone

        it "should parse a null field as None" <| fun _ ->
            // Act
            let actual = JsonAdapter.getObjOption "null" exampleJson

            // Assert
            expect actual <| isNone

        it "should raise an exception for a field of type array" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getObjOption "array" exampleJson

        it "should raise an exception for a field of type string" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getObjOption "string" exampleJson

        it "should raise an exception for a field of type float" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getObjOption "float" exampleJson

        it "should raise an exception for a field of type int" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getObjOption "int" exampleJson

        it "should raise an exception for a dot-delimited field name" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getObjOption "field.with.dots" exampleJson

    describe "getObject" <| fun _ ->

        it "should parse an object field" <| fun _ ->
            // Act
            let actual = JsonAdapter.getObj "obj" exampleJson

            // Assert
            expect actual?field <| equalTo "value"

        it "should parse an empty object field" <| fun _ ->
            // Act / Test succeeds if no exception thrown
            JsonAdapter.getObj "emptyObj" exampleJson
            |> ignore

        it "should raise an exception for a missing field" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getObj "missing" exampleJson

        it "should raise an exception for a null field" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getObj "null" exampleJson

        it "should raise an exception for a field of type array" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getObj "array" exampleJson

        it "should raise an exception for a field of type string" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getObj "string" exampleJson

        it "should raise an exception for a field of type float" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getObj "float" exampleJson

        it "should raise an exception for a field of type int" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getObj "int" exampleJson

    describe "getArrayOption" <| fun _ ->

        it "should parse an array field as Some" <| fun _ ->
            // Act
            let actual = JsonAdapter.getArrayOption "array" exampleJson

            // Assert
            expect actual <| isSome
            expect actual.Value.Length <| equalTo 2

        it "should parse an empty array field as Some" <| fun _ ->
            // Act
            let actual = JsonAdapter.getArrayOption "emptyArray" exampleJson

            // Assert
            expect actual <| isSome
            expect actual.Value.Length <| equalTo 0

        it "should parse a missing field as None" <| fun _ ->
            // Act
            let actual = JsonAdapter.getArrayOption "missing" exampleJson

            // Assert
            expect actual <| isNone

        it "should parse a null field as None" <| fun _ ->
            // Act
            let actual = JsonAdapter.getArrayOption "null" exampleJson

            // Assert
            expect actual <| isNone

        it "should raise an exception for a field of type object" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getArrayOption "obj" exampleJson

        it "should raise an exception for a field of type string" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getArrayOption "string" exampleJson

        it "should raise an exception for a field of type float" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getArrayOption "float" exampleJson

        it "should raise an exception for a field of type int" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getArrayOption "int" exampleJson

    describe "getArray" <| fun _ ->

        it "should parse an array field" <| fun _ ->
            // Act
            let actual = JsonAdapter.getArray "array" exampleJson

            // Assert
            expect actual.Length <| equalTo 2

        it "should parse an empty array field" <| fun _ ->
            // Act
            let actual = JsonAdapter.getArray "emptyArray" exampleJson

            // Assert
            expect actual.Length <| equalTo 0

        it "should raise an exception for a missing field" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getArray "missing" exampleJson

        it "should raise an exception for a null field" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getArray "null" exampleJson

        it "should raise an exception for a field of type object" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getArray "obj" exampleJson

        it "should raise an exception for a field of type string" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getArray "string" exampleJson

        it "should raise an exception for a field of type float" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getArray "float" exampleJson

        it "should raise an exception for a field of type int" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getArray "int" exampleJson

    describe "getStringOption" <| fun _ ->

        it "should parse a string field as Some" <| fun _ ->
            // Act
            let actual = JsonAdapter.getStringOption "string" exampleJson

            // Assert
            expect actual <| isSome
            expect actual.Value <| equalTo "a string"

        it "should parse an empty string field as Some" <| fun _ ->
            // Act
            let actual = JsonAdapter.getStringOption "emptyString" exampleJson

            // Assert
            expect actual <| isSome
            expect actual.Value <| equalTo ""

        it "should parse a missing field as None" <| fun _ ->
            // Act
            let actual = JsonAdapter.getStringOption "missing" exampleJson

            // Assert
            expect actual <| isNone

        it "should parse a null field as None" <| fun _ ->
            // Act
            let actual = JsonAdapter.getStringOption "null" exampleJson

            // Assert
            expect actual <| isNone

        it "should raise an exception for a field of type object" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getStringOption "obj" exampleJson

        it "should raise an exception for a field of type array" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getStringOption "array" exampleJson

        it "should raise an exception for a field of type float" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getStringOption "float" exampleJson

        it "should raise an exception for a field of type int" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getStringOption "int" exampleJson

    describe "getString" <| fun _ ->

        it "should parse a string field" <| fun _ ->
            // Act
            let actual = JsonAdapter.getString "string" exampleJson

            // Assert
            expect actual <| equalTo "a string"

        it "should parse an empty string field" <| fun _ ->
            // Act
            let actual = JsonAdapter.getString "emptyString" exampleJson

            // Assert
            expect actual <| equalTo ""

        it "should raise an exception for a missing field" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getString "missing" exampleJson

        it "should raise an exception for a null field" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getString "null" exampleJson

        it "should raise an exception for a field of type object" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getString "obj" exampleJson

        it "should raise an exception for a field of type array" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getString "array" exampleJson

        it "should raise an exception for a field of type float" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getString "float" exampleJson

        it "should raise an exception for a field of type int" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getString "int" exampleJson

    describe "getFloatOption" <| fun _ ->

        it "should parse a float field as Some" <| fun _ ->
            // Act
            let actual = JsonAdapter.getFloatOption "float" exampleJson

            // Assert
            expect actual <| isSome
            expect actual.Value <| equalTo 10.569845

        it "should parse an int field as Some" <| fun _ ->
            // Act
            let actual = JsonAdapter.getFloatOption "int" exampleJson

            // Assert
            expect actual <| isSome
            expect actual.Value <| equalTo 123.0

        it "should parse a missing field as None" <| fun _ ->
            // Act
            let actual = JsonAdapter.getFloatOption "missing" exampleJson

            // Assert
            expect actual <| isNone

        it "should parse a null field as None" <| fun _ ->
            // Act
            let actual = JsonAdapter.getFloatOption "null" exampleJson

            // Assert
            expect actual <| isNone

        it "should raise an exception for a field of type object" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getFloatOption "obj" exampleJson

        it "should raise an exception for a field of type array" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getFloatOption "array" exampleJson

        it "should raise an exception for a field of type string" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getFloatOption "string" exampleJson

        it "should raise an exception for a field of type string (empty)" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getFloatOption "emptyString" exampleJson

    describe "getFloat" <| fun _ ->

        it "should parse a float field" <| fun _ ->
            // Act
            let actual = JsonAdapter.getFloat "float" exampleJson

            // Assert
            expect actual <| equalTo 10.569845

        it "should parse an int field" <| fun _ ->
            // Act
            let actual = JsonAdapter.getFloat "int" exampleJson

            // Assert
            expect actual <| equalTo 123.0

        it "should raise an exception for a missing field" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getFloat "missing" exampleJson

        it "should raise an exception for a null field" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getFloat "null" exampleJson

        it "should raise an exception for a field of type object" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getFloat "obj" exampleJson

        it "should raise an exception for a field of type array" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getFloat "array" exampleJson

        it "should raise an exception for a field of type string" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getFloat "string" exampleJson

        it "should raise an exception for a field of type string (empty)" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getFloat "emptyString" exampleJson

    describe "getIntOption" <| fun _ ->

        it "should parse an int field as Some" <| fun _ ->
            // Act
            let actual = JsonAdapter.getIntOption "int" exampleJson

            // Assert
            expect actual <| isSome
            expect actual.Value <| equalTo 123

        it "should parse a float field as Some (floor value)" <| fun _ ->
            // Act
            let actual = JsonAdapter.getIntOption "float" exampleJson

            // Assert
            expect actual <| isSome
            expect actual.Value <| equalTo 10

        it "should parse a missing field as None" <| fun _ ->
            // Act
            let actual = JsonAdapter.getIntOption "missing" exampleJson

            // Assert
            expect actual <| isNone

        it "should parse a null field as None" <| fun _ ->
            // Act
            let actual = JsonAdapter.getIntOption "null" exampleJson

            // Assert
            expect actual <| isNone

        it "should raise an exception for a field of type object" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getIntOption "obj" exampleJson

        it "should raise an exception for a field of type array" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getIntOption "array" exampleJson

        it "should raise an exception for a field of type string" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getIntOption "string" exampleJson

        it "should raise an exception for a field of type string (empty)" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getIntOption "emptyString" exampleJson

    describe "getInt" <| fun _ ->

        it "should parse an int field" <| fun _ ->
            // Act
            let actual = JsonAdapter.getInt "int" exampleJson

            // Assert
            expect actual <| equalTo 123

        it "should parse a float field (floor value)" <| fun _ ->
            // Act
            let actual = JsonAdapter.getInt "float" exampleJson

            // Assert
            expect actual <| equalTo 10

        it "should raise an exception for a missing field" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getInt "missing" exampleJson

        it "should raise an exception for a null field" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getInt "null" exampleJson

        it "should raise an exception for a field of type object" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getInt "obj" exampleJson

        it "should raise an exception for a field of type array" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getInt "array" exampleJson

        it "should raise an exception for a field of type string" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getInt "string" exampleJson

        it "should raise an exception for a field of type string (empty)" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getInt "emptyString" exampleJson

    describe "getIsoDateTimeOffsetOption" <| fun _ ->

        it "should parse a valid ISO datetime string as Some" <| fun _ ->
            // Act
            let actual = JsonAdapter.getIsoDateTimeOffsetOption "iso_date_time" exampleJson
            let expected = DateTimeOffset.Parse("2019-05-22T02:29:00.1347954Z")

            // Assert
            expect actual <| isSome
            expect actual.Value <| equalTo expected

        it "should parse a missing field as None" <| fun _ ->
            // Act
            let actual = JsonAdapter.getIsoDateTimeOffsetOption "missing" exampleJson

            // Assert
            expect actual <| isNone

        it "should parse a null field as None" <| fun _ ->
            // Act
            let actual = JsonAdapter.getIsoDateTimeOffsetOption "null" exampleJson

            // Assert
            expect actual <| isNone

        it "should raise an exception for an invalid date time format" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getIsoDateTimeOffsetOption "invalid_date_time" exampleJson

    describe "getIsoDateTimeOffset" <| fun _ ->

        it "should parse a valid ISO datetime string" <| fun _ ->
            // Act
            let actual = JsonAdapter.getIsoDateTimeOffset "iso_date_time" exampleJson
            let expected = DateTimeOffset.Parse("2019-05-22T02:29:00.1347954Z")

            // Assert
            expect actual <| equalTo expected

        it "should raise an exception for a null field" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getIsoDateTimeOffset "missing" exampleJson

        it "should raise an exception for a null field" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getIsoDateTimeOffset "null" exampleJson

        it "should raise an exception for an invalid date time format" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getIsoDateTimeOffset "invalid_date_time" exampleJson
                
    describe "getBooleanOption" <| fun _ ->

        it "should parse true as Some true" <| fun _ ->
            // Act
            let actual = JsonAdapter.getBooleanOption "boolTrue" exampleJson
            let expected = Some true

            // Assert
            expect actual <| equalTo expected

        it "should parse false as Some false" <| fun _ ->
            // Act
            let actual = JsonAdapter.getBooleanOption "boolFalse" exampleJson
            let expected = Some false

            // Assert
            expect actual <| equalTo expected

        it "should parse a missing field as None" <| fun _ ->
            // Act
            let actual = JsonAdapter.getBooleanOption "missing" exampleJson

            // Assert
            expect actual <| isNone

        it "should parse a null field as None" <| fun _ ->
            // Act
            let actual = JsonAdapter.getBooleanOption "null" exampleJson

            // Assert
            expect actual <| isNone

    describe "getBoolean" <| fun _ ->

        it "should parse true as true" <| fun _ ->
            // Act
            let actual = JsonAdapter.getBoolean "boolTrue" exampleJson
            let expected = true

            // Assert
            expect actual <| equalTo expected

        it "should parse false as false" <| fun _ ->
            // Act
            let actual = JsonAdapter.getBoolean "boolFalse" exampleJson
            let expected = false

            // Assert
            expect actual <| equalTo expected

        it "should raise an exception for a missing field" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getBoolean "missing" exampleJson

        it "should raise an exception for a null field" <| fun _ ->
            // Act / Assert
            expectException <| fun _ ->
                JsonAdapter.getBoolean "null" exampleJson
