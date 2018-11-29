namespace Hcs.Platform.Data
{
    public class GetKeyAndModelOutput<TKey, TEntity>
    {
        public TKey Key { get; internal set; }
        public TEntity Entity { get; internal set; }
    }
    public class GetKeyAndModelOutput<T, TKey, TEntity> : GetKeyAndModelOutput<TKey, TEntity>
    {
        public T Input { get; internal set; }
    }
    public class GetKeyAndModel<TKey, TEntity> : Flow.FlowProcessor<object, GetKeyAndModelOutput<TKey, TEntity>>
    {
        private readonly Core.KeyRequestContext<TKey> key;
        private readonly Core.InputRequestContext<TEntity> inputModel;

        public GetKeyAndModel(Core.KeyRequestContext<TKey> key, Core.InputRequestContext<TEntity> model)
        {
            this.key = key;
            this.inputModel = model;
        }
        public override GetKeyAndModelOutput<TKey, TEntity> Process(object input)
        {
            return new GetKeyAndModelOutput<TKey, TEntity>
            {
                Key = key.Key,
                Entity = inputModel.Input
            };
        }
    }
    public class GetKeyAndModel<T, TKey, TEntity> : Flow.FlowProcessor<T, GetKeyAndModelOutput<T, TKey, TEntity>>
    {
        private readonly Core.KeyRequestContext<TKey> key;
        private readonly Core.InputRequestContext<TEntity> inputModel;

        public GetKeyAndModel(Core.KeyRequestContext<TKey> key, Core.InputRequestContext<TEntity> model)
        {
            this.key = key;
            this.inputModel = model;
        }
        public override GetKeyAndModelOutput<T, TKey, TEntity> Process(T input)
        {
            return new GetKeyAndModelOutput<T, TKey, TEntity>
            {
                Input = input,
                Key = key.Key,
                Entity = inputModel.Input
            };
        }
    }
}