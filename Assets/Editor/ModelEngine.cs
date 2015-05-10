using UnityEngine;
using UnityEditor;
using FFMProject;
using System.Collections;

[CustomEditor(typeof(EditableObject))]
public class ModelEngine : Editor
{
	public static ModelEngine the;
	public EditableObject editableObj;
    public Material material = null;
    public Camera camera = null;
	public FFMProject.Model model { get { return (Model)editableObj.model; } }
	public Mesh mesh { get { return editableObj.meshfilter.sharedMesh; } }

    static ModelEngine()
    {
        EditorApplication.update += Update;
    }

    static void Update()
    {
    }

    public void OnEnable()
    {
		editableObj = (EditableObject)target; 
		the = this;
		editableObj.DrawEventHandlers += new FFMProject.DrawDelegate(OnDraw);
		editableObj.ModelCreator = ModelCreator;
    }

    public void OnDisable()
    {
		editableObj.DrawEventHandlers -= new FFMProject.DrawDelegate(OnDraw);
		editableObj.ModelCreator = null;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(" Grid Width ");
        GUILayout.EndHorizontal();
    }

	public ModelData ModelCreator()
	{
		return new Model();
	}

    void OnSceneGUI()
    {
        camera = Camera.current;
        if (camera == null)
            return;

        Event e = Event.current;

//      if (e.type == EventType.KeyDown && e.keyCode == KeyCode.A)
//      if (e.type == EventType.KeyUp && e.keyCode == KeyCode.A)

        // This line prevents any other objects from being selected.
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

        if (e.button == 0)
        {
            if (e.type == EventType.MouseDown)
            {
                currentTool_.OnLMBDown(this);
                e.Use();
            }

            if (e.type == EventType.MouseDrag)
            {
                currentTool_.OnMouseDrag(this);
                e.Use();
            }

            if (e.type == EventType.MouseUp)
            {
                currentTool_.OnLMBUp(this);
                e.Use();
            }
        }

        if (e.type == EventType.MouseMove)
        {
            currentTool_.OnMouseMove(this);
            e.Use();
        }

		currentTool_.Update(this);
    }

	public void UpdateAll()
	{
		MeshCompiler.Compile (model, mesh);
	}

    public void OnDraw()
    {
		currentTool_.Display(this);
    }

    public Ray ViewRay()
    {
        return HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
    }

    public bool Raycast(out RaycastHit hit)
    {
        return Physics.Raycast(ViewRay(), out hit);
    }

    public void HitPos(out Vector3 hitPos, Vector3 pos)
    {
        RaycastHit hit;
        if( Raycast(out hit) )
        {
            hitPos = hit.point;
            return;
        }
        hitPos = pos;
    }

    private BaseTool currentTool_ = new BoxTool();
}
