# My Wardrobe

## Notable Design Aspects
- Can add images to clothing items
  - By storing a filepath and saving the image to the host's project folder
  - Deleting an image will also delete the image file from the image folder
  - Images are stored in a way that they are organised by their clothing item id
    - <image folder>/<clothing item id>/<filename>
    - This ensures that if two items use the same image file, deleting the image from one of them
      will not affect the image of the other item

## To Run:
1. Open the solution file with Visual Studio
2. Run the following commands in Package Manager Console
    - 'dotnet restore'
    - 'Add-Migration InitialCreate'
    - 'Update-Database'
3. Startup the project Debug -> Start Without Debugging (or Ctrl + F5)

