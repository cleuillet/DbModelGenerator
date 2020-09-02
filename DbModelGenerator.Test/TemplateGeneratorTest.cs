using System.Collections.Immutable;
using NUnit.Framework;

namespace DbModelGenerator.Test
{
    public sealed class TemplateGeneratorTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GenerateAClassForOneTable()
        {
            var table = new Table("user_profile",
                new[] {new Column("id", "string", false, true, false)}.ToImmutableList());

            var actual = TemplateGenerator.GenerateClass("Project.Generated.Global", table, null, null, null, "db");

            const string expected = @"
namespace Project.Generated.Global
{

	public sealed class UserProfileDb
	{

		public UserProfileDb(string id)
		{
			Id = id;
		}

		public string Id { get; }

	}

}";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GenerateAClassForOneTableWithUsing()
        {
            var table = new Table("user_profile",
                new[] {new Column("id", "Guid", false, true, false)}.ToImmutableList());

            var actual = TemplateGenerator.GenerateClass("Project.Generated.Global", table, null, null, null, "Db");

            const string expected = @"using System;

namespace Project.Generated.Global
{

	public sealed class UserProfileDb
	{

		public UserProfileDb(Guid id)
		{
			Id = id;
		}

		public Guid Id { get; }

	}

}";
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void GenerateAClassForOneTableWithUsingAndIdentity()
        {
            var table = new Table("user_profile",
                new[] {new Column("id", "Guid", false, true, false)}.ToImmutableList());

            var actual =
                TemplateGenerator.GenerateClass("Project.Generated.Global", table, "Odin.Api.IIdentity", null, null,
                    "Db");

            const string expected = @"using System;
using Odin.Api;

namespace Project.Generated.Global
{

	public sealed class UserProfileDb : IIdentity<Guid>
	{

		public UserProfileDb(Guid id)
		{
			Id = id;
		}

		public Guid Id { get; }

	}

}";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GenerateAClassForOneTableWithUsingAndIdentityWithoutId()
        {
            var table = new Table("user_profile",
                new[]
                {
                    new Column("role_id", "Guid", false, true, false), new Column("group_id", "int", false, true, false)
                }.ToImmutableList());

            var actual =
                TemplateGenerator.GenerateClass("Project.Generated.Global", table, "Odin.Api.IIdentity;Odin.Api.IRoleEntity(role_id);Odin.Api.IGroupEntity(role_id,group_id!);Odin.Api.Entity.IDbEntity(model_id,created_by,creation_date,modified_by,modification_date)", null, null,
                    null);

            const string expected = @"using System;
using Odin.Api;

namespace Project.Generated.Global
{

	public sealed class UserProfile : IRoleEntity, IGroupEntity<int>
	{

		public UserProfile(Guid role_id, int group_id)
		{
			RoleId = role_id;
			GroupId = group_id;
		}

		public Guid RoleId { get; }

		public int GroupId { get; }

	}

}";
            Assert.AreEqual(expected, actual);
        }
    }
}