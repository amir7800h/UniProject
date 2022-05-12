using Application.Discounts.AddNewDiscountService;
using MD.PersianDateTime;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Admin.EndPoint.Binders
{
    public class DiscountEntityBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            string FieldName = bindingContext.FieldName;

            AddNewDiscountDto discountDto = new AddNewDiscountDto()
            {
                CouponCode = bindingContext.ValueProvider
                .GetValue($"{FieldName}.{nameof(discountDto.CouponCode)}").Values.ToString(),

                
                DiscountAmount = int.Parse(bindingContext.ValueProvider
                .GetValue($"{FieldName}.{nameof(discountDto.DiscountAmount)}").Values.ToString()),

                DiscountLimitationId = int.Parse(bindingContext.ValueProvider
                .GetValue($"{FieldName}.{nameof(discountDto.DiscountLimitationId)}").Values.ToString()),

                DiscountPercentage = int.Parse(bindingContext.ValueProvider
                .GetValue($"{FieldName}.{nameof(discountDto.DiscountPercentage)}").Values.ToString()),

                DiscountTypeId = int.Parse(bindingContext.ValueProvider
                .GetValue($"{FieldName}.{nameof(discountDto.DiscountTypeId)}").Values.ToString()),

                LimitationTimes = int.Parse(bindingContext.ValueProvider
                .GetValue($"{FieldName}.{nameof(discountDto.LimitationTimes)}").Values.ToString()),


                UsePercentage = bool.Parse(bindingContext.ValueProvider
                .GetValue($"{FieldName}.{nameof(discountDto.UsePercentage)}").FirstValue.ToString()),

                Name = bindingContext.ValueProvider
                .GetValue($"{FieldName}.{nameof(discountDto.Name)}").Values.ToString(),

                RequiresCouponCode = bool.Parse(bindingContext.ValueProvider
                .GetValue($"{FieldName}.{nameof(discountDto.RequiresCouponCode)}").FirstValue.ToString()),

                EndDate = PersianDateTime.Parse(bindingContext.ValueProvider
                .GetValue($"{FieldName}.{nameof(discountDto.EndDate)}").Values.ToString()),

                StartDate = PersianDateTime.Parse(bindingContext.ValueProvider
                .GetValue($"{FieldName}.{nameof(discountDto.StartDate)}").Values.ToString()),
            };

            var appliedToCatalogItem = bindingContext.ValueProvider
                .GetValue($"{FieldName}.{nameof(discountDto.appliedToCatalogItem)}");

            if (!string.IsNullOrEmpty(appliedToCatalogItem.Values))
            {
                discountDto.appliedToCatalogItem =
                    bindingContext.ValueProvider
                    .GetValue($"{FieldName}.{nameof(discountDto.appliedToCatalogItem)}")
                    .Values.ToString().Split(',').Select(x => Int32.Parse(x)).ToList();
            }

            bindingContext.Result = ModelBindingResult.Success(discountDto);
            return Task.CompletedTask;
        }
    }
}
