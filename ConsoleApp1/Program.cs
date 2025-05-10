// See https://aka.ms/new-console-template for more information

using ConsoleApp1;
using Specification.Evaluators;
using Specification.Interfaces;

ISpecification<User> a = new UserCombineSpec();
IQueryable<User> user = Enumerable.Empty<User>().AsQueryable();
var b = SpecificationEvaluator.GetQuery<User>(user, a);