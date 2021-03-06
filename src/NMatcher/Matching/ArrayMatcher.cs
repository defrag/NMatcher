﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMatcher.Matching
{
    public sealed class ArrayMatcher : IMatcher
    {
        public Result Match(object value)
        {
            var res = value.GetType().IsArray;

            return res ? Result.Success() : Result.Failure($"{value} is not a valid array.");
        }
    }
}
