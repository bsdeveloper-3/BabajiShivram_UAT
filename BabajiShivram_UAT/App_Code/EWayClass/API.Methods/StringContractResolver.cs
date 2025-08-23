
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Reflection;

public sealed class SubstituteNullWithEmptyStringContractResolver : DefaultContractResolver
{
  protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
  {
    JsonProperty property = base.CreateProperty(member, memberSerialization);
    if (property.PropertyType == typeof (string))
      property.ValueProvider = (IValueProvider) new SubstituteNullWithEmptyStringContractResolver.NullToEmptyStringValueProvider(property.ValueProvider);
    return property;
  }

  private sealed class NullToEmptyStringValueProvider : IValueProvider
  {
    private readonly IValueProvider Provider;

    public NullToEmptyStringValueProvider(IValueProvider provider)
    {
      if (provider == null)
        throw new ArgumentNullException("provider");
      this.Provider = provider;
    }

    public object GetValue(object target)
    {
      return this.Provider.GetValue(target) ?? (object) "";
    }

    public void SetValue(object target, object value)
    {
      this.Provider.SetValue(target, value);
    }
  }
}
