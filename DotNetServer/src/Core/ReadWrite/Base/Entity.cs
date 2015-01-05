using System;
using System.Data;
using System.Data.SqlClient;

namespace Core.ReadWrite.Base
{
    public abstract class Entity : IEntity
    {
        protected Entity()
        {
            Id = Guid.Empty;
        }

        public virtual bool IsPersistent
        {
            get { return IsPersistentObject(); }
        }

        public virtual Guid Id { get; set; }

        public virtual void From(IDataReader dataReader)
        {
            Id = dataReader.ReadUid("Id");
        }

        public virtual void To(SqlCommand cmd)
        {
            cmd.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = Id;
        }

        public override bool Equals(object obj)
        {
            if (IsPersistentObject())
            {
                var persistentObject = obj as Entity;
                return (persistentObject != null) && (IdsAreEqual(persistentObject));
            }

            return base.Equals(obj);
        }

        protected bool IdsAreEqual(Entity entity)
        {
            return Equals(Id, entity.Id);
        }

        public override int GetHashCode()
        {
            return IsPersistentObject() ? Id.GetHashCode() : base.GetHashCode();
        }

        private bool IsPersistentObject()
        {
            return !Equals(Id, Guid.Empty);
        }
    }
}