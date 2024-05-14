using DataPersistence;
using UnityEngine;

namespace UI.SettingsManagement {
    public abstract class BaseFieldView : MonoBehaviour {

        [SerializeField] public FieldName fieldName;

        public abstract void Subscribe();
        public abstract void Unsubscribe();
    }
}
