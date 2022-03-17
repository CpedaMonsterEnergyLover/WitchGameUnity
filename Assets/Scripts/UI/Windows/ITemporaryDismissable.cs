public interface ITemporaryDismissable {
    public void SetActive(bool isActive) { }
    public bool IsActive { get; }
}