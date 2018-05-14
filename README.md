## DynamicMap
Dynamic mapping of object to anonymous type with support of Json.Net and complex objects, Similar to AutoMapper library but with support of dynamic types.

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

Note that Mapper accepts optional parameter of type: `Dictionary<Type, Func<object, object>>` which can be used to get original object property value manually.
