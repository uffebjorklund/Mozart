using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Mozart.Helpers
{
    /// <summary>
    /// These helpers should only be used when loading types into memory.
    /// After first load the optimal thing is to use the compiled expressions.
    /// </summary>
    public static class ReflectionHelpers
    {
        //public static IDictionary<Type,Type> GenericExportInstances = new Dictionary<Type, Type>(); 

        /// <summary>
        /// Find the first exportable Ctor (maybe this should use ImportingCtor attr ?)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ConstructorInfo GetExportableConstructor(this Type type)
        {
            var expCtor = type.GetConstructors().FirstOrDefault(c =>c.CtorIsExportable());

            if (expCtor != null) return expCtor;

            return type.GetConstructors().FirstOrDefault();
        }

        /// <summary>
        /// Check that the Ctor contains interface parameters only
        /// </summary>
        /// <param name="ctorInfo"></param>
        /// <returns></returns>
        public static bool CtorIsExportable(this ConstructorInfo ctorInfo)
        {
            return ctorInfo.GetParameters()
                .All(p => p.ParameterType.IsInterface && p.ParameterType.IsExported()) && ctorInfo.GetParameters().Any();
        }

        public static object[] GetCtorParameters(this ConstructorInfo ctorInfo)
        {
            if(ctorInfo == null) return new object[0];

            var p = ctorInfo.GetParameters();

            if (!p.Any()) return new object[0];

            return (from parameterInfo in p 
                    let export = parameterInfo.ParameterType.MakeGenericExport() 
                    let method = export.GetMethod("Get", new Type[] {typeof (Type)}) 
                    select method.Invoke(export, new object[] {parameterInfo.ParameterType})).ToArray();
        }

        public static bool IsExported(this Type type)
        {
            return Compose.InstrumentFactory.ContainsKey(type);
            //var sasas = (ss.Key == type);
            //var exported = Compose.ExportedInterfaceFactory.ContainsKey(type);
            //return exported;
            //Type export = type.MakeGenericExport();

            //var getMethod = export.GetMethod("Get", new Type[] { typeof(Type) });

            //return getMethod.Invoke(export, new object[] { type }) != null;
        }

        public static Type MakeGenericExport(this Type t)
        {
            //TODO: Only do this once and then save the constructed item in a list

            if(!t.IsInterface) throw new Exception("Something that makes sense should be written here");

            //if (GenericExportInstances.ContainsKey(t))
            //    return GenericExportInstances[t];

            Type export = typeof(Compose<>);
            
            Type[] typeArgs = { t };

            Type constructed = export.MakeGenericType(typeArgs);

            var getMethod = constructed.GetMethod("Init");

            getMethod.Invoke(constructed,null);

            //Activator.CreateInstance(constructed, new []{t});
            //GenericExportInstances.Add(t,constructed);

            return constructed;
        }

        /// <summary>
        /// Will return the instance of T
        /// 
        /// The export will decide if it is a singleton or a multiple instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetInstance<T>()
        {   
            return (T)typeof(T).GetInstance();
        }

        /// <summary>
        /// Should only be used when creating obejct the first time...
        /// 
        /// Should also make expressions and compile to delegate for re-usage.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object GetInstance(this Type type)
        {
            var ctorInfo = type.GetExportableConstructor();
            if (ctorInfo == null) return Activator.CreateInstance(type);
            var parameters = ctorInfo.GetCtorParameters();
            return ctorInfo.Invoke(parameters.Select(parameter => parameter.GetType().GetInstance()).ToArray());
        }
    }
}
