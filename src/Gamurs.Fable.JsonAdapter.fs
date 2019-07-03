namespace Gamurs.Fable.JsonAdapter

/// A simple pipeline-based API for adapting POJO fields to F# types, with
/// type validation.
module JsonAdapter =

    open System
    open Fable.Core
    open Fable.Core.JsInterop
    open Fable.Import

    // Use inheritance to generate custom exceptions rather than F# syntax -
    // which doesn't populate exception.message. Creates issues with interop.

    /// Exception representing a failure to parse the json object.
    type JsonParsingException(msg : string) =
        inherit Exception(msg)

    [<Emit("Math.floor($0)")>]
    let private jsFloor number =  jsNative

    module private TypePredicate =
        [<Emit("typeof $0 === 'number'")>]
        let isNumber obj : bool = jsNative

        [<Emit("typeof $0 === 'string'")>]
        let isString obj : bool = jsNative

        [<Emit("typeof $0 === 'boolean'")>]
        let isBoolean obj : bool = jsNative

        // Use JS.isArray for arrays. typeof will return object for arrays.
        let isArray obj : bool =
            JS.Array.isArray obj

        [<Emit("typeof $0 === 'object'")>]
        let private isObjectNative obj : bool = jsNative

        let isObject obj : bool =
            // Javascript arrays have type object
            isObjectNative obj && not (isArray obj)

    [<Emit("typeof $0")>]
    let private typeof obj : string = jsNative

    /// <summary>
    ///     Unbox a successful parsing result, or raise a
    ///     <c>JsonParsingException</c> on an Error result.
    /// </summary>
    ///
    /// <param name="result">
    ///     The result to unbox.
    /// </param>
    ///
    /// <returns>
    ///     Successfully parsed value.
    /// </returns>
    let raiseOnError<'a> (result : Result<'a, string>) : 'a =
        match result with
        | Ok value ->
            value
        | Error errorMsg ->
            errorMsg
            |> JsonParsingException
            |> raise

    let private validateType
        (typePredicate : obj -> bool)
        (fieldName : string)
        (value : 'a)
        : 'a =

        match typePredicate value with
        | true ->
            value
        | false ->
            let actualType = typeof obj
            sprintf
                "JSON field '%s' did not have expected type.  Actual type is: %s"
                fieldName
                actualType
            |> JsonParsingException
            |> raise

    /// Ensure that a requried field is not None
    let private validateRequired
        (fieldName : string)
        (value : 'a option)
        : 'a =

        match value with
        | Some v ->
            v
        | None ->
            sprintf
                "Required JSON field '%s' was missing."
                fieldName
            |> JsonParsingException
            |> raise


    /// <summary>
    ///     Get the value of the specified field of the provided Javascript
    ///     object, ensuring that the value matches the provided type predicate.
    /// </summary>
    ///
    /// <param name="typePredicate">
    ///     Predicate which takes in the object, and asserts on the type.
    ///     One of the above defined functions:
    ///
    ///     <see cref="isArray">isString</see>
    ///     <see cref="isBoolean">isString</see>
    ///     <see cref="isNumber">isNumber</see>
    ///     <see cref="isObject">isString</see>
    ///     <see cref="isString">isString</see>
    ///     etc.
    /// </param>
    ///
    /// <param name="fieldName">
    ///     The name of the field.
    /// </param>
    ///
    /// <param name="parentObj">
    ///     The parent js-object from which to get the field.
    /// </param>
    ///
    /// <returns>
    ///     The object field if it passed the type-validation and was not null
    ///     undefined.
    /// </returns>
    let private getFieldOption<'a when 'a : null>
        (typePredicate : obj -> bool)
        (fieldName : string)
        (parentObj : obj) : 'a option =

        // Dot delimited fields are not supported
        if fieldName.Contains(".") then
            sprintf "Dot-delimited field name not suported: %s" fieldName
            |> JsonParsingException
            |> raise

        parentObj?(fieldName)
        |> Option.ofObj
        |> Option.map (validateType typePredicate fieldName)

    /// Get a requred field, raise an exception if it is null or undefined.
    let private getField<'a when 'a : null>
        (typePredicate : obj -> bool)
        (fieldName : string)
        (parentObj : obj) : 'a =

        getFieldOption
            typePredicate
            fieldName
            parentObj
        |> (validateRequired fieldName)

    /// <summary>
    ///     Get the value of an object field from a Javascript object.
    ///     Raises an exception if the field contains a value of type other than
    ///     object.
    /// </summary>
    ///
    /// <param name="fieldName">
    ///     The name of the field to get.
    /// </param>
    ///
    /// <param name="parentObj">
    ///     The Javascript object from which to get the field.
    /// </param>
    ///
    /// <returns>
    ///     The optional value of the field if it is an object.
    /// </returns>
    let getObjOption
        (fieldName : string)
        (parentObj : obj) : obj option =

        getFieldOption
            TypePredicate.isObject
            fieldName
            parentObj

    /// <summary>
    ///     Get the value of an object field from a Javascript object.
    ///     Raises an exception if the field does not contain a value of type
    ///     object.
    /// </summary>
    ///
    /// <param name="fieldName">
    ///     The name of the field to get.
    /// </param>
    ///
    /// <param name="parentObj">
    ///     The Javascript object from which to get the field.
    /// </param>
    ///
    /// <returns>
    ///     The value of the field if it is an object.
    /// </returns>
    let getObj
        (fieldName : string)
        (parentObj : obj) : obj =

        getObjOption
            fieldName
            parentObj
        |> (validateRequired fieldName)

    /// <summary>
    ///     Get the value of an array field from a Javascript object.
    ///     Raises an exception if the field contains a value of type other than
    ///     array.
    /// </summary>
    ///
    /// <param name="fieldName">
    ///     The name of the field to get.
    /// </param>
    ///
    /// <param name="parentObj">
    ///     The Javascript object from which to get the field.
    /// </param>
    ///
    /// <returns>
    ///     The optional value of the field if it is an array.
    /// </returns>
    let getArrayOption
        (fieldName : string)
        (parentObj : obj) : 'a list option =

        getFieldOption
            TypePredicate.isArray
            fieldName
            parentObj
        |> Option.map List.ofArray

    /// <summary>
    ///     Get the value of an array field from a Javascript object.
    ///     Raises an exception if the field does not contain a value of type
    ///     array.
    /// </summary>
    ///
    /// <param name="fieldName">
    ///     The name of the field to get.
    /// </param>
    ///
    /// <param name="parentObj">
    ///     The Javascript object from which to get the field.
    /// </param>
    ///
    /// <returns>
    ///     The value of the field if it is an array.
    /// </returns>
    let getArray
        (fieldName : string)
        (parentObj : obj) : 'a list =

        getArrayOption
            fieldName
            parentObj
        |> (validateRequired fieldName)

    /// <summary>
    ///     Get the value of a boolean field from a Javascript object.
    ///     Raises an exception if the field contains a value of type other than
    ///     boolean.
    /// </summary>
    ///
    /// <param name="fieldName">
    ///     The name of the field to get.
    /// </param>
    ///
    /// <param name="parentObj">
    ///     The Javascript object from which to get the field.
    /// </param>
    ///
    /// <returns>
    ///     The optional value of the field if it is a boolean.
    /// </returns>
    let getBooleanOption
        (fieldName : string)
        (parentObj : obj) : bool option =

        getFieldOption
            TypePredicate.isBoolean
            fieldName
            parentObj

    /// <summary>
    ///     Get the value of a boolean field from a Javascript object.
    ///     Raises an exception if the field does not contain a value of type
    ///     boolean.
    /// </summary>
    ///
    /// <param name="fieldName">
    ///     The name of the field to get.
    /// </param>
    ///
    /// <param name="parentObj">
    ///     The Javascript object from which to get the field.
    /// </param>
    ///
    /// <returns>
    ///     The value of the field if it is a boolean.
    /// </returns>
    let getBoolean
        (fieldName : string)
        (parentObj : obj) : bool =

        getBooleanOption
            fieldName
            parentObj
        |> (validateRequired fieldName)

    /// <summary>
    ///     Get the value of a string field from a Javascript object.
    ///     Raises an exception if the field contains a value of type other than
    ///     string.
    /// </summary>
    ///
    /// <param name="fieldName">
    ///     The name of the field to get.
    /// </param>
    ///
    /// <param name="parentObj">
    ///     The Javascript object from which to get the field.
    /// </param>
    ///
    /// <returns>
    ///     The optional value of the field if it is a string.
    /// </returns>
    let getStringOption
        (fieldName : string)
        (parentObj : obj) : string option =

        getFieldOption
            TypePredicate.isString
            fieldName
            parentObj

    /// <summary>
    ///     Get the value of a string field from a Javascript object.
    ///     Raises an exception if the field does not contain a value of type
    ///     string.
    /// </summary>
    ///
    /// <param name="fieldName">
    ///     The name of the field to get.
    /// </param>
    ///
    /// <param name="parentObj">
    ///     The Javascript object from which to get the field.
    /// </param>
    ///
    /// <returns>
    ///     The value of the field if it is a string.
    /// </returns>
    let getString
        (fieldName : string)
        (parentObj : obj) : string =

        getStringOption
            fieldName
            parentObj
        |> (validateRequired fieldName)

    /// <summary>
    ///     Get the value of a float field from a Javascript object.
    ///     Raises an exception if the field contains a value of type other than
    ///     float.
    /// </summary>
    ///
    /// <param name="fieldName">
    ///     The name of the field to get.
    /// </param>
    ///
    /// <param name="parentObj">
    ///     The Javascript object from which to get the field.
    /// </param>
    ///
    /// <returns>
    ///     The optional value of the field if it is a float.
    /// </returns>
    let getFloatOption
        (fieldName : string)
        (parentObj : obj) : float option =

        getFieldOption
            TypePredicate.isNumber
            fieldName
            parentObj
        |> Option.map (fun number ->
            number :> obj :?> float)

    /// <summary>
    ///     Get the value of a float field from a Javascript object.
    ///     Raises an exception if the field does not contain a value of type
    ///     float.
    /// </summary>
    ///
    /// <param name="fieldName">
    ///     The name of the field to get.
    /// </param>
    ///
    /// <param name="parentObj">
    ///     The Javascript object from which to get the field.
    /// </param>
    ///
    /// <returns>
    ///     The value of the field if it is a float.
    /// </returns>
    let getFloat
        (fieldName : string)
        (parentObj : obj) : float =

        getFloatOption
            fieldName
            parentObj
        |> (validateRequired fieldName)

    /// <summary>
    ///     Get the value of an int field from a Javascript object.
    ///     Raises an exception if the field contains a value of type other than
    ///     int.
    /// </summary>
    ///
    /// <param name="fieldName">
    ///     The name of the field to get.
    /// </param>
    ///
    /// <param name="parentObj">
    ///     The Javascript object from which to get the field.
    /// </param>
    ///
    /// <returns>
    ///     The optional value of the field if it is an int.
    let getIntOption
        (fieldName : string)
        (parentObj : obj) : int option =

        getFieldOption
            TypePredicate.isNumber
            fieldName
            parentObj
        |> Option.map (fun number ->
            number :> obj :?> int)
        |> Option.map jsFloor

    /// <summary>
    ///     Get the value of an int field from a Javascript object.
    ///     Raises an exception if the field does not contain a value of type
    ///     int.
    /// </summary>
    ///
    /// <param name="fieldName">
    ///     The name of the field to get.
    /// </param>
    ///
    /// <param name="parentObj">
    ///     The Javascript object from which to get the field.
    /// </param>
    ///
    /// <returns>
    ///     The value of the field if it is an int.
    /// </returns>
    let getInt
        (fieldName : string)
        (parentObj : obj) : int =

        getIntOption
            fieldName
            parentObj
        |> (validateRequired fieldName)

    /// <summary>
    ///     Get the value of an ISO formatted date time offset field from a
    ///     Javascript object. Raises an exception if the field contains a value
    ///     of type other than string, or if the string is not a valid
    ///     DateTimeOffset.
    /// </summary>
    ///
    /// <param name="fieldName">
    ///     The name of the field to get.
    /// </param>
    ///
    /// <param name="parentObj">
    ///     The Javascript object from which to get the field.
    /// </param>
    ///
    /// <returns>
    ///     The optional value of the field if it is successfully parsed to a
    ///     DateTimeOffset.
    /// </returns>
    let getIsoDateTimeOffsetOption
        (fieldName : string)
        (parentObj : obj) : DateTimeOffset option =

        let validateDateTimeOffset (valid, offset) : DateTimeOffset =
            match valid, offset with
            | true, validOffset ->
                validOffset
            | false, _ ->
                sprintf
                    "Invalid DateTimeOffset: %s"
                    fieldName
                |> JsonParsingException
                |> raise

        getStringOption
            fieldName
            parentObj
        |> Option.map DateTimeOffset.TryParse
        |> Option.map validateDateTimeOffset

    /// <summary>
    ///     Get the value of an ISO formatted date time offset field from a
    ///     Javascript object. Raises an exception if the field contains a value
    ///     of type other than string, or if the string is not a valid
    ///     DateTimeOffset.
    /// </summary>
    ///
    /// <param name="fieldName">
    ///     The name of the field to get.
    /// </param>
    ///
    /// <param name="parentObj">
    ///     The Javascript object from which to get the field.
    /// </param>
    ///
    /// <returns>
    ///     The successfully parsed DateTimeOffset.
    /// </returns>
    let getIsoDateTimeOffset
        (fieldName : string)
        (parentObj : obj) : DateTimeOffset =

        getIsoDateTimeOffsetOption
            fieldName
            parentObj
        |> (validateRequired fieldName)
