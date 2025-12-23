using Lotus.Repository;

namespace Lotus.Web
{
    /** \addtogroup WebCommonDevice
	*@{*/
    /// <summary>
    /// Сервис для работы с устройством входа.
    /// </summary>
    public class DeviceService : ILotusDeviceService
    {
        #region Fields
        private readonly ILotusDataStorage _dataStorage;
        #endregion

        #region Constructors
        /// <summary>
        /// Конструктор инициализирует объект класса указанными параметрами.
        /// </summary>
        /// <param name="dataStorage">Интерфейс для работы с сущностями.</param>
        public DeviceService(ILotusDataStorage dataStorage)
        {
            _dataStorage = dataStorage;
        }
        #endregion

        #region ILotusDeviceService methods
        /// <inheritdoc/>
        public async Task<Device> GetOrAddAsync(Device? device, CancellationToken token)
        {
            var devices = _dataStorage.Query<Device>();
            if (device == null)
            {
                if (devices.Any())
                {
                    return devices.First();
                }
                else
                {
                    var newDevice = new Device();
                    newDevice.SetCodeId();
                    await _dataStorage.AddAsync(newDevice, token);
                    await _dataStorage.SaveChangesAsync(token);

                    return newDevice;
                }
            }

            var findDevice = devices.Where(x => x.CodeId == device.CodeId).FirstOrDefault();
            if (findDevice is not null)
            {
                return findDevice;
            }

            device.SetCodeId();
            await _dataStorage.AddAsync(device, token);
            await _dataStorage.SaveChangesAsync(token);

            return device;
        }
        #endregion
    }
    /**@}*/
}