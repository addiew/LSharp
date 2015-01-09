﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CLRSharp
{
    class Type_Common_System : ICLRType
    {
        public System.Type TypeForSystem
        {
            get;
            private set;
        }
        public Type_Common_System(System.Type type)
        {
            this.TypeForSystem = type;
        }
        public string Name
        {
            get { return TypeForSystem.Name; }
        }

        public string FullName
        {
            get { return TypeForSystem.FullName; }
        }

        public IMethod GetMethod(string funcname, MethodParamList types)
        {
            if (funcname == ".ctor")
            {
                var con = TypeForSystem.GetConstructor(types.ToArraySystem());
                return new Method_Common_System(con);
            }
            var method = TypeForSystem.GetMethod(funcname, types.ToArraySystem());
            return new Method_Common_System(method);
        }
        public IField GetField(string name)
        {
            return new Field_Common_System(TypeForSystem.GetField(name));
        }
    }
    class Field_Common_System : IField
    {
        System.Reflection.FieldInfo info;
        public Field_Common_System(System.Reflection.FieldInfo field)
        {
            info = field;
        }

        public void Set(object _this, object value)
        {
            info.SetValue(_this, value);
        }

        public object Get(object _this)
        {
            return info.GetValue(_this);
        }

        public bool isStatic
        {
            get { return info.IsStatic; }
        }
    }

    class Method_Common_System : IMethod
    {

        public Method_Common_System(System.Reflection.MethodBase method)
        {
            if (method == null)
                throw new Exception("not allow null method.");
            method_System = method;
        }
        public bool isStatic
        {
            get { return method_System.IsStatic; }
        }

        public System.Reflection.MethodBase method_System;

        public object Invoke(ThreadContext context, object _this, object[] _params)
        {

            if (method_System is System.Reflection.ConstructorInfo)
            {
                var newobj = (method_System as System.Reflection.ConstructorInfo).Invoke(_params);
                return newobj;
            }
            else
            {
                return method_System.Invoke(_this, _params);
            }

        }



    }

}