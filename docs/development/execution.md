# Order of Execution

A mod's core event methods are called at the same time as the game manager's core events, so this chart shows the order in which they are called

---

```mermaid
---
title: Startup
---
graph TD
    A[Mod - OnPreInitialize]
    B([Manager - Initialize])
    C[Mod - OnInitialize]
    D[Mod - OnRegisterServices]
    E([Manager - AllPreInitialized])
    F([Manager - AllInitialized])
    G[Mod - OnAllInitialized]
    H(GlobalData - Load)

    A-->B
    B-->C
    C-->D
    D-->E
    E-->F
    F-->G
    G-->H

mermaid.flowchartConfig = {
    width: 50%
}
```

```mermaid
---
title: Shutdown
---
graph TD
    A(GlobalData - Save)
    B[Mod - OnDispose]
    C([Manager - Dispose])

    A-->B
    B-->C

mermaid.flowchartConfig = {
    width: 50%
}
```