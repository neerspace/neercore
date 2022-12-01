Usage
=====

Installation
------------

To use Lumache, first install it using pip:

Creating recipes
----------------

To retrieve a list of random ingredients, you can use the function:

[//]: # ()
[//]: # (::: lumache.get_random_ingredients)

[//]: # (    options:)

[//]: # (      show_root_heading: true)

<br>

The `kind` parameter should be either `"meat"`, `"fish"`, or `"veggies"`.
Otherwise, will raise an exception [`lumache.InvalidKindError`](/api#lumache.InvalidKindError).

For example:
