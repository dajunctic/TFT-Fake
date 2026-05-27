using System;

namespace DTT.Utils.Workflow
{
   /// <summary>
   /// Represents an action that is only executed once.
   /// </summary>
   /// <typeparam name="T">The argument type.</typeparam>
   public class SingleAction<T>
   {
      /// <summary>
      /// The action to execute.
      /// </summary>
      private readonly Action<T> _action;

      /// <summary>
      /// Whether the action is invoked.
      /// </summary>
      private bool _invoked;

      /// <summary>
      /// Initializes the action.
      /// </summary>
      /// <param name="action">The action to execute once.</param>
      public SingleAction(Action<T> action) => _action = action;

      /// <summary>
      /// Invokes the action if not already invoked once.
      /// </summary>
      /// <param name="argument">The argument for the action.</param>
      public void Invoke(T argument)
      {
         if (_invoked)
            return;

         _action.Invoke(argument);
         _invoked = true;
      }
   }

   /// <summary>
   /// Represents an action that is only executed once.
   /// </summary>
   public class SingleAction
   {
      /// <summary>
      /// The action to execute.
      /// </summary>
      private readonly Action _action;

      /// <summary>
      /// Whether the action is invoked.
      /// </summary>
      private bool _invoked;

      /// <summary>
      /// Initializes the action.
      /// </summary>
      /// <param name="action">The action to execute once.</param>
      public SingleAction(Action action) => _action = action;
      
      /// <summary>
      /// Invokes the action if not already invoked once.
      /// </summary>
      public void Invoke()
      {
         if (_invoked)
            return;

         _action.Invoke();
         _invoked = true;
      }
   }
}
