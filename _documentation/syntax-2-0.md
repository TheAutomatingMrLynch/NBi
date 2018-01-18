---
layout: documentation
title: Syntax 2.0
permalink: /docs/syntax-2-0/
---
NBi was created in 2012 (as a New Year's resolution) and is supporting the same syntax in the last six years. Unfortunately, I made some poor decisions during the design of a few features. That’s the problem when you’re working on your free time: it’s not always possible to be smart when it’s 1.00AM or when you’ve been interrupted ten times by your daughters, your wife or sport at TV during the last 30 minutes :-) Anyway, it starts to be cumbersome to go forward and to make improvements to the tool without fixing these design issues. To fix these issues, some of the xml elements will need to be changed.  On the long-term, some of the syntaxes currently supported by NBi will not work anymore but we hope that the new syntax will be more user-friendly.

## PascalCase vs dashes

Some of the names of the xml elements are written in Pascal-case and not with dashes between words. This will be adapted along the way to the release 2.0 Until this moment both syntaxes will be supported. The current status is visible in this [issue](http://github.com/Seddryck/NBi/issues/288)

## *execution* replaced by *result-set*

The system-under-test *execution* was a bit ambiguous, sometimes used for performance or successfulness of a query/ETL and sometimes for the result-set of a query. The new syntax will clarify this by introducing the system-under-test *result-set*.

This result-set can be defined in different way. The most straightforward is to define rows and cells inline. 

{% highlight xml %}
<resultSet>
  <row>
    <cell>Canada</cell>
    <cell>130</cell>
  </row>
  <row>
    <cell>Belgium</cell>
    <cell>45</cell>
  </row>
</resultSet>
{% endhighlight %}

You can also refer to an external CSV file:

{% highlight xml %}
<resultSet file="myFile.csv"/>
{% endhighlight %}

Or through a query. This query can be sourced from an inline definition

{% highlight xml %}
<resultSet>
  <query>
    select * from myTable
  </query>
<resultSet>
{% endhighlight %}

Or an external file

{% highlight xml %}
<resultSet>
  <query file="myQuery.sql"/>
<resultSet>
{% endhighlight %}

Or an [assembly](../docs/query-assembly)

{% highlight xml %}
<resultSet>
  <query>
    <assembly ...>
  <query>
<resultSet>
{% endhighlight %}

or a [report](../docs/query-report#dataset)

{% highlight xml %}
<resultSet>
  <query>
    <report ...>
  <query>
<resultSet>
{% endhighlight %}

or a [shared-dataset](../docs/shared-dataset)

{% highlight xml %}
<resultSet>
  <query>
    <shared-dataset ...>
  <query>
<resultSet>
{% endhighlight %}

You can also define an alteration to the result-set. For the moment, two kinds of alterations are supported by NBi:

* [filter](../resultset-rows-count-advanced/#filter).
* [convert](../resultset-alterations/#converts)

{% highlight xml %}
<resultSet>
  <query>
    ...
  <query>
  <alteration>
    <filter ...>
  </alteration>
<resultSet>
{% endhighlight %}