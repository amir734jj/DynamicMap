## DynamicMap
Dynamic mapping of object to anonymous type with support of Json.Net and complex objects, Similar to AutoMapper library but with support of dynamic types.

[Nuget packge](https://www.nuget.org/packages/DynamicMap/1.0.0)

```csharp
// Create new dummy object
var obj = new DummyClass();

// Wrap `obj` to JObject
var json = JObject.FromObject(obj);
            
// Map json to DummyNestedClass
var result = DynamicMap.Map(typeof(DummyNestedClass), json);
            
// Assert result is equal to original object
Assert.Equal(result, obj);
```

## Changes as of version 2.0
I re-designed the whole library in version 2.0, it is more modular now. It is not more similar to popular mapping library, [AutoMapper](https://automapper.org/). This is in way comparable to the extensive features of AutoMapper but it gets the job done for simple POCOs, even nested ones with complex IEnumerable properties.

As of version 2.0, to add a custom mapping profile, library requires creating a class that extends `ISpecialMapper`. In the example below, I extended `BaseDynamicMap` as I did not want to implement many other interface methods as well.

```csharp
public class CustomClassSpecialMapper: BaseDynamicMap, ISpecialMapper
{
  // returns new instance of this mapper
  public new ISpecialMapper New() => new CustomClassSpecialMapper();

  // if true then this mapper will be used
  public bool MatchingMapper(Type destinationType, Type sourceType, object sourceObj)
  {
      return sourceType == typeof(CustomClass);
  }
  
  // order of custom mapper
  public int Order() => 4;
}
```

Finally, register the custom dynamic mapper

```csharp
DynamicMap.GetDynamicMapBuilder().RegisterCustomMapper(new CustomClassSpecialMapper()); 
```

## Supporting types:
- Primitve types
- List and IEnumerables
- JObject (`JSON.net`'s dynamic object)
- ExpandoObjects (essentially `IDictionary<string, object>`)

Note that when `IsComplex` flag is set to `true` on a property of a type, then mapper will run a dynamic map recursively for that specific property. Please see `BaseDynamicMap.cs` for more details.


## Release notes:
- Added support for mapping of primitives
- Added support for `IDictionary`
- Added support for merge array of objects into one type