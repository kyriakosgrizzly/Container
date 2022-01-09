
using System;
using System.Collections.Generic;
using System.Reflection;

namespace demo_DI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IoContainer container = new Container();

            Init(container);
            Test1(container);
            Test2(container);
            Test3(container);
            Test4(container);

            Console.ReadKey();
        }
        //Home work
        class Container: IoContainer
        {
            public Dictionary<object,object> list = new Dictionary<object,object>();
            
            public void Register<T>() where T : class
            {
                list.Add(typeof(T), typeof(T));
            }
            public void Register<T,R>() where R : class, T
            {
                if (typeof(T).IsAssignableFrom(typeof(R))){
                    list.Add(typeof(T),typeof(R));
                }
            }
            public void Register<T>(Func<T> factory)
            {
                list.Add(typeof(T), factory().GetType());
            }
            public T Resolve<T>()
            {

                if (list.TryGetValue(typeof(T), out object tmp))
                {
                    return (T)Activator.CreateInstance((System.Type)tmp);
                }
                throw new Exception();
            }
        }
        static void Init(IoContainer container)
        {
            container.Register<Test>();
            container.Register<IUserEntity,User>();
            container.Register<IDirectLink, DirectLink>();
            container.Register<IForDelegate>(() =>
            {
                return new ForDelegate();
            });

            //can be many...
        }

        static void Test2(IoContainer container)
        {
            var userEntity = container.Resolve<IUserEntity>();
            Console.WriteLine(userEntity.Ping());
        }

        static void Test1(IoContainer container)
        {
            var test = container.Resolve<Test>();
            Console.WriteLine(test.Log());
        }

        static void Test3(IoContainer container)
        {
            var test = container.Resolve<IDirectLink>();
            Console.WriteLine(test.Ping());
        }

        static void Test4(IoContainer container)
        {
            var test = container.Resolve<IForDelegate>();
            Console.WriteLine(test.SomeMethod());
        }
    }

    public class User : IUserEntity
    {
        public string Ping()
        {
            return "User Ping!";
        }
        public User()
        {

        }
    }
    public class Test
    {
        public string Log()
        {
            return "Test object!";
        }
        public Test()
        {

        }
    }
    public class ForDelegate : IForDelegate
    {
        public string SomeMethod()
        {
            return "ForDelegate!";
        }
    }

    public class DirectLink : IDirectLink
    {
        public string Ping()
        {
            return "DirectLink !";
        }
    }

    public interface IUserEntity
    {
        string Ping();
    }

    public interface IoContainer
    {
        void Register<T>()
            where T : class;

        void Register<T, R>()
            where R : class, T;

        void Register<T>(Func<T> factory);

        /// <summary>
        /// Resolve function T type can be class or interface
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Resolve<T>();
    }
    public interface IForDelegate
    {
        string SomeMethod();
    }
    public interface IDirectLink
    {
        string Ping();
    }

}