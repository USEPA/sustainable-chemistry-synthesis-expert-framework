using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SustainableChemistry
{

    public class SelectionChangedEventArgs : System.EventArgs
    {

        GraphicObject m_SelectedObject;
        GraphicObject[] m_SelectedObjects;

        public SelectionChangedEventArgs(GraphicObject selectedObject)
        {
            m_SelectedObject = selectedObject;
            m_SelectedObjects = new GraphicObject[0];
        }


        public SelectionChangedEventArgs(GraphicObject[] selectedObjects)
        {
            m_SelectedObject = null;
            if (selectedObjects.Length > 0) m_SelectedObject = selectedObjects[0];
            m_SelectedObjects = selectedObjects;
        }

        public Object SelectedObject
        {
            get
            {
                return m_SelectedObject;
            }
        }

        public Object SelectedObjects
        {
            get
            {
                return m_SelectedObjects;
            }
        }
    };

    public enum StatusUpdateType
    {
        ObjectRotated = 0,
        ObjectMoved = 1,
        ObjectDeleted = 2,
        SurfaceZoomChanged = 3,
        FileLoaded = 4,
        FileSaved = 5,
        SelectionChanged = 6
    };

    public class StatusUpdateEventArgs : System.EventArgs
    {
        StatusUpdateType m_UpdateType;
        GraphicObject m_SelectedObject;
        GraphicObject[] m_SelectedObjects;
        String m_Message;
        System.Drawing.Point m_Coord;
        double m_Amount;

        public StatusUpdateEventArgs(StatusUpdateType UpdateType, GraphicObject Selection, String StatusMessage, System.Drawing.Point Coord, double Amt)
        {
            m_UpdateType = UpdateType;
            m_SelectedObject = Selection;
            m_SelectedObjects = new GraphicObject[1];
            m_SelectedObjects[0] = Selection;
            m_Message = StatusMessage;
            m_Coord = Coord;
            m_Amount = Amt;
        }


        public StatusUpdateEventArgs(StatusUpdateType UpdateType, GraphicObject[] Selection, String StatusMessage, System.Drawing.Point Coord, double Amt)
        {
            m_UpdateType = UpdateType;
            m_SelectedObject = null;
            if (Selection.Length > 0) m_SelectedObject = Selection[0];
            m_SelectedObjects = Selection;
            m_Message = StatusMessage;
            m_Coord = Coord;
            m_Amount = Amt;
        }

        public StatusUpdateType Type
        {
            get
            {
                return m_UpdateType;
            }
        }

        public GraphicObject SelectedObject
        {
            get
            {
                return m_SelectedObject;
            }
        }

        public GraphicObject[] SelectedObjects
        {
            get
            {
                return m_SelectedObjects;
            }
        }

        public String Message
        {
            get
            {
                return m_Message;
            }
        }
        public System.Drawing.Point Coordinates
        {
            get
            {
                return m_Coord;
            }
        }

        public double Amount
        {
            get
            {
                return m_Amount;
            }
        }
    };

    public enum GraphicObjectsChangedType
    {
        Edited = 0,
        Added = 1,
        Cut = 2,
        Copied = 3,
        Pasted = 4,
        Deleted = 5,
        DeleteAll = 6
    };


    public class GraphicObjectsChangedEventsArgs : System.EventArgs
    {
        Object m_ChangedObject;
        GraphicObjectsChangedType m_ChangeType;

        public GraphicObjectsChangedEventsArgs(Object newObject, GraphicObjectsChangedType type)
        {
            m_ChangedObject = newObject;
            m_ChangeType = type;
        }

        public Object ChangedObject
        {
            get
            {
                return m_ChangedObject;
            }
        }

        public GraphicObjectsChangedType ChangeType
        {
            get
            {
                return m_ChangeType;
            }
        }
    };
}
