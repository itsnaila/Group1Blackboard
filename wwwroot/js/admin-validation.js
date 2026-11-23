(function ($) {
    "use strict";

    $.validator.addMethod("pastbirthdate", function (value, element, bounds) {
        return this.optional(element) ||
            /^\d{4}-\d{2}-\d{2}$/.test(value) && value >= bounds.min && value <= bounds.max;
    });

    $.validator.unobtrusive.adapters.add("pastbirthdate", ["min", "max"], function (options) {
        options.rules.pastbirthdate = options.params;
        options.messages.pastbirthdate = options.message;
    });

    $(function () {
        $(".admin-validation-form").each(function () {
            var form = $(this);
            var validator = form.data("validator");
            var summary = form.find("[data-client-validation-summary]");

            // Match MVC's treatment of whitespace-only form values.
            validator.settings.normalizer = function (value) {
                return value.trim() === "" ? "" : value;
            };

            form.on("input change", "[data-val=true]:not([data-val-remote])", function () {
                validator.element(this);
            });

            // Check uniqueness on blur, including an untouched value in an edit form.
            form.on("focusout", "[data-val-remote]", function () {
                if (!validator.submitted[this.name] && !validator.invalid[this.name]) {
                    validator.element(this);
                }
            });

            form.on("invalid-form", function () {
                summary.prop("hidden", false);
            });
        });
    });
}(jQuery));
