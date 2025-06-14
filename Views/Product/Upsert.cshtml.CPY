
@model Rocky_Models.ViewModels.ProductVM

@{
 var title = "Create Product";
}
  



<form method="post" enctype="multipart/form-data">
    @if(Model.Product.Id != null){
        title = "Edit Product";
        <input asp-for="Product.Id" hidden />
    }
    
    <div class="border p-3">
        <div class="form-group row">
            <h2 class="text-info pl-3">@title</h2>
        </div>
   
        <div class="row">
            <div class="col-8">
                <div class="form-group row p-3">
                    <div class="col-4">
                       <label asp-for="Product.Name"></label>
                    </div>
                    <div class="col-8">
                       <input asp-for="Product.Name" class="form-control" />
                       <span asp-validation-for="Product.Name" class="text-danger"></span>
                    </div>
                </div>
                <div class="form-group row p-3">
                    <div class="col-4">
                        <label asp-for="Product.Price"></label>
                    </div>
                    <div class="col-8">
                         <input asp-for="Product.Price" class="form-control" />
                         <span asp-validation-for="Product.Price" class="text-danger"></span>
                    </div>
                </div>
                <div class="form-group row p-3">
                    <div class="col-4">
                        <label asp-for="Product.Description"></label>
                    </div>
                    <div class="col-8">
                         <textarea asp-for="Product.Description" class="form-control summernote" ></textarea>
                         <span asp-for="Product.Description" class="text-danger"></span>
                    </div>
                </div>
                  <div class="form-group row p-3">
                    <div class="col-4">
                        Image
                    </div>
                    <div class="col-8">
                         <input type="file" name="files" id="uploadBox" multiple class="form-control" />
                    </div>
                </div>
                 <div class="form-group row p-3">
                    <div class="col-4">
                        <label asp-for="Product.CategoryId" ></label>
                    </div>
                    <div class="col-8">
                         <select asp-validation-for="Product.CategoryId"  asp-items="@Model.CategorySelectList" class="form-control">
                            <option disabled >--Select Category--</option>
                         </select>
                         <span asp-for="Product.CategoryId" class="text-danger"></span>
                    </div>
                </div>
                <div class="form-group row p-3">
                    <div class="col-8 offset-4 row">
                        <div class="col">
                            @if(Model.Product.Id!=null){
                                //update
                                <input type="submit" class="btn btn-info w-100" value="Update" />
                            }else{
                                //create
                                <input type="submit" onclick="return validateInput()" class="btn btn-primary w-100" value="Create" />
                            }
                        </div>
                        <div class="col">
                            <a asp-action="Index" class="btn btn-success"><i class="fa-solid fa-right-from-bracket"></i></a>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-4">
                @* Keep this empty *@
                @if(Model.Product.Id != null){
                <img src="@WC.ImagePath@Model.Product.Image" width="100%" style="border-radius:5px; border:1px solid #bbb" />
                }
            </div>
        </div>
    </div>
</form>

 @section Scripts{
    @{<partial name="_ValidationScriptsPartial"/>}
 
    <script>
    
      $(document).ready(function() {
            $('.summernote').summernote({
                 height:250
            });
        });     
          function validateInput(){
            if(document.getElementById("uploadBox").value==""){
               
                Swal.fire({
                    title: "Error !",
                    text: "Please upload an Image!",
                    icon: "error"
                });
                return false;
            }
            return true;
        } 
     
</script>
}
