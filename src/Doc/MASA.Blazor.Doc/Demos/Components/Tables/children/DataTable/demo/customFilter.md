---
order: 1
title:
  zh-CN: 自定义过滤器
  en-US: CustomFilter
---

## zh-CN

你可以向 CustomFilter 属性提供一个函数，覆盖 Search 属性的默认过滤。 如果你需要自定义特定列的过滤，你可以给表头数据项的 Filter 属性提供一个函数。 类型是`Func<object, string, TItem, bool>`。 即使没有提供 Search 属性，这个函数也会一直运作。 因此，你需要确保在不应用过滤器的情况下，函数会返回 true。

## en-US

Todo...
