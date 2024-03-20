using _1_MapsterQuickStart;
using Mapster;
using MapsterMapper;
using System.Threading.Channels;

var user = new User()
{
    Id = 3,
    FirstName = "Jane",
    LastName = "ThereIsLastName"
};


//1. 手动映射
/*var userResponse = new UserResponse()
{
    Id = user.Id,
    FirstName = user.FirstName,
    LastName = user.LastName
};*/

/*
 * 2.1 使用 Mapster 的默认映射
 * 目标对象的属性必须包含源对象的全部属性(且同名)，否则报错
 * 目标对象可以含源对象没有的属性(不会映射，但也不报错)
 */
//var userResponse = user.Adapt<UserResponse>();

//太麻烦，不推荐
/*UserResponse userResponse = new();
user.Adapt(userResponse, typeof(User) ,typeof(UserResponse));*/

/*
 * 2.2 使用 TypeAdapterConfig 自定义映射配置
 * 仅配置部分属性的映射时，不会对其他属性造成影响
 * 或者使用 .IgnoreNonMapped(true) 来忽略未配置属性
 */
/*var config = new TypeAdapterConfig();
config.NewConfig<User, UserResponse>()
    .Map(dest => dest.FullName, src => $"{src.FirstName}.{src.LastName}")
    .IgnoreNonMapped(true);

var userResponse = user.Adapt<UserResponse>(config);*/

/*
 * 2.3 使用 GlobalSettings 定义映射配置
 * 查看源码可知，默认映射实际上是传入了 GlobalSettings 这个公共的全局设置
 * 
 */
/*var config = TypeAdapterConfig.GlobalSettings;
config.NewConfig<User, UserResponse>()
    .Map(dest => dest.FullName, src => $"{src.FirstName}.{src.LastName}")
    .IgnoreNonMapped(false);

var userResponse = user.Adapt<UserResponse>(config);*/

/*
 * 2.4 使用 TypeAdapterConfig<TSource, TDestination>.NewConfig 静态方法定义配置
 * 此方法其实就是 Mapster 内部使用 2.3 中的方法修改了 GlobalSettings，因此映射时不需要再传入 config
 */
/*TypeAdapterConfig<User, UserResponse>.NewConfig()
    .Map(dest => dest.FullName, src => $"{src.FirstName}.{src.LastName}")
    .IgnoreNonMapped(false);

var userResponse = user.Adapt<UserResponse>();*/

/*
 * 2.5 使用 Mapper 类的实例对象定义配置
 * Mapper 的构造函数中仍然是简单地使用全局设置，因此所有对 GlobalSettings 的设置都会影响它
 */
/*IMapper mapper = new Mapper();

var userResponse = mapper.Map<UserResponse>(user);*/

/*
 * 3.1 多次使用 .NewConfig 时，会覆盖之前的配置
 * 注：此时对于未配置的属性将不会映射，而不管是否有 .IgnoreNonMapped(true)
 */
/*var config = TypeAdapterConfig.GlobalSettings;
config.NewConfig<User, UserResponse>()
    .Map(dest => dest.FullName, src => $"{src.FirstName}.{src.LastName}");

config.NewConfig<User, UserResponse>()
    .Map(dest => dest.Id, src => src.Id + 1);

var userResponse = user.Adapt<UserResponse>(config);*/

/*
 * 3.2 使用 .ForType 附加配置，而不覆盖原有配置
 * 注：.ForType只能附加而不能覆盖，即使
 */
/*var config = TypeAdapterConfig.GlobalSettings;
config.NewConfig<User, UserResponse>()
    .Map(dest => dest.FullName, src => $"{src.FirstName}.{src.LastName}");

config.ForType<User, UserResponse>()
    .Map(dest => dest.Id, src => src.Id + 1);

var userResponse = user.Adapt<UserResponse>(config);*/

/*
 * 4. 使用条件语句限制是否映射
 */
/*var config = TypeAdapterConfig.GlobalSettings;
config.NewConfig<User, UserResponse>()
    .Map(
    dest => dest.FullName,
    src => $"{src.FirstName}.{src.LastName}",
    src => src.Id == 4);

var userResponse = user.Adapt<UserResponse>(config);*/

/*
 * 5. 多个源对象映射成一个目标对象
 * 
 */
//模拟全局唯一Id
/*var traceId = Guid.NewGuid();

//将 User 和 Guid 一同映射到 UserResponse 上
TypeAdapterConfig<(User User, Guid TraceId), UserResponse>.NewConfig()
    .Map(dest => dest.TraceId, src => src.TraceId)
    .Map(dest => dest.Id, src => src.User.Id);

var userResponse = (user, traceId).Adapt<UserResponse>();*/

/*
 * 6. 使用 BeforeMapping / AfterMapping 在映射前/后立即调用
 */
/*var traceId = Guid.NewGuid();

//将 User 和 Guid 一同映射到 UserResponse 上
TypeAdapterConfig<(User User, Guid TraceId), UserResponse>.NewConfig()
    .Map(dest => dest.TraceId, src => src.TraceId)
    .Map(dest => dest.Id, src => src.User.Id)
    .BeforeMapping(_ => Console.WriteLine("Something Before Mapping..."))
    .AfterMapping(_ => Console.WriteLine("Something After Mapping..."));

var userResponse = (user, traceId).Adapt<UserResponse>();*/

/*
 * 7. 使用 ForDestinationType 调用目标对象的方法
 */
var traceId = Guid.NewGuid();

var config = TypeAdapterConfig.GlobalSettings;

TypeAdapterConfig<(User User, Guid TraceId), UserResponse>.NewConfig()
    .Map(dest => dest.TraceId, src => src.TraceId)
    .Map(dest => dest.Id, src => src.User.Id);

config.ForDestinationType<IValidatable>()
    .AfterMapping(dest => dest.Validate());

var userResponse = (user, traceId).Adapt<UserResponse>();

Console.WriteLine(user);
Console.WriteLine(userResponse);

public interface IValidatable
{
    //.Net 8.0
    void Validate()
    {
        Console.WriteLine("Validating...");
    }
}
