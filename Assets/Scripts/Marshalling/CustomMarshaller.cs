using System;
interface CustomMarshaller
{
    IntPtr NativeDataPointer { get; set; }
    object GetMangedObject();
    void MarshalNativeToManaged();
    void DeleteNativePointer();
}