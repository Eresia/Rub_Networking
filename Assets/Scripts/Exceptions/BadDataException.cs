using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization;

public class BadDataException: Exception {

  public BadDataException()
    : base() {
  }

  public BadDataException(String message)
    : base(message) {
  }

  public BadDataException(String message, Exception innerException)
    : base(message, innerException) {
  }

  protected BadDataException(SerializationInfo info, StreamingContext context)
    : base(info, context) {
  }
}