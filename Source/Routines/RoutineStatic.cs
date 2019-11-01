using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Omega.Routines
{
    public partial class Routine
    {
        [NotNull]
        public static Routine Delay(float interval)
        {
            if (interval < 0 || float.IsNaN(interval))
                throw new ArgumentOutOfRangeException(nameof(interval));

            return new DelayRoutine(interval);
        }

        [NotNull]
        public static TaskRoutine Task([NotNull] Action action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return new TaskRoutine(action);
        }

        [NotNull]
        public static TaskRoutine<TResult> Task<TResult>([NotNull] Func<TResult> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return new TaskRoutine<TResult>(action);
        }

        [NotNull]
        public static GroupRoutine WhenAll([NotNull] IEnumerable<Routine> routines)
        {
            if (routines == null)
                throw new ArgumentNullException();

            return new GroupRoutine(routines);
        }

        [NotNull]
        public static GroupRoutine WhenAll([NotNull] params Routine[] routines)
        {
            if (routines == null)
                throw new ArgumentNullException(nameof(routines));

            return new GroupRoutine(routines);
        }

        public static Routine<T> FromResult<T>(T result) => new FromResultRoutine<T>(result);

        public static EnumeratorAsRoutine ByEnumerator(IEnumerator enumerator)
        {
            if(enumerator == null)
                throw new ArgumentNullException(nameof(enumerator));
            
            return new EnumeratorAsRoutine(enumerator);
        }

        public static EnumeratorAsRoutineWithController ByEnumerator(Func<RoutineControl, IEnumerator> enumeratorGetter)
        {
            if(enumeratorGetter == null)
                throw new ArgumentNullException(nameof(enumeratorGetter));
            
            return new EnumeratorAsRoutineWithController(enumeratorGetter);
        }

        public static EnumeratorAsRoutineWithResult<T> ByEnumerator<T>(
            Func<RoutineControl<T>, IEnumerator> enumeratorGetter)
        {
            if(enumeratorGetter == null)
                throw new ArgumentNullException(nameof(enumeratorGetter));
            
            return new EnumeratorAsRoutineWithResult<T>(enumeratorGetter);
        }
    }
}