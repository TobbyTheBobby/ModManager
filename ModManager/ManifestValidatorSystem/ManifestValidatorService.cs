using System.Collections.Generic;
using Timberborn.SingletonSystem;

namespace ModManager.ManifestValidatorSystem
{
    public class ManifestValidatorService : ILoadableSingleton
    {
        private readonly IEnumerable<IManifestValidator> _manifestValidators;

        public ManifestValidatorService(IEnumerable<IManifestValidator> manifestValidators)
        {
            _manifestValidators = manifestValidators;
        }

        public void Load()
        {
            foreach (var validator in _manifestValidators)
            {
                validator.ValidateManifests();
            }
        }
    }
}
