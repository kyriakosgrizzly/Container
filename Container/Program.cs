
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
            public List<object> list = new List<object>();
            
            public void Register<T>() where T : class
            {
                list.Add((T)Activator.CreateInstance(typeof(T)));
            }
            public void Register<T,R>() where R : class, T
            {
                if (typeof(T).IsAssignableFrom(typeof(R))){
                    list.Add((T)Activator.CreateInstance(typeof(R)));
                }
            }
            public void Register<T>(Func<T> factory)
            {
                list.Add(factory());
            }
            public T Resolve<T>()
            {
                foreach(var item in list)
                {
                    if (typeof(T).IsAssignableFrom(item.GetType()))
                    {
                        return (T)item;
                    }
                }
                throw new Exception();
            }
        }
        static void Init(IoContainer container)
        {
            container.Register<Test>();
            container.Register<User>();
            container.Register<IDirectLink, DirectLink>();
            container.Register<IForDelegate>(() =>
            {
                return new ForDelegate();
            });

            //can be many...
        }

        static void Test1(IoContainer container)
        {
            var userEntity = container.Resolve<IUserEntity>();
            Console.WriteLine(userEntity.Ping());
        }

        static void Test2(IoContainer container)
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
    }
    public class Test
    {
        public string Log()
        {
            return "Test object!";
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