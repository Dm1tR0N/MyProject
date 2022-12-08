using System.Diagnostics;
using Avalonia.Controls;
using KinoPoisk2.Views;

namespace UnitTests;

public class UnitTest1
{
    [Fact]
    public void SearchBtn_Testing()
    {
        // arange
        string textForTest = "A. O. Scott joined The New York Times as a film critic in January 2000," +
                             " and was named a chief critic in 2004. Previously, " +
                             "Mr. Scott had been the lead Sunday book reviewer for Newsday and a frequent contributor to Slate, " +
                             "The New York Review of Books, and many other publications. " +
                             "\n<br/><br/>\n" +
                             "In the 1990s he served on the editorial staffs of Lingua Franca and The New York Review of Books. He also edited" +
                             " \"A Bolt from the Blue and Other Essays,\" " +
                             "a collection by Mary McCarthy, which was published by The New York Review of Books in 2002." +
                             " \n<br/><br/>\nMr. Scott was a finalist for the Pulitzer Prize in Criticism in 2010, " +
                             "the same year he served as co-host (with Michael Phillips of the Chicago Tribune) on the last season of" +
                             " \"At the Movies,\" the syndicated film-reviewing program started by Roger Ebert and Gene Siskel." +
                             "\n<br/><br/>\nA frequent presence on radio and television, Mr. Scott is Distinguished Professor" +
                             " of Film Criticism at Wesleyan University and the author of Better Living Through Criticism, " +
                             "forthcoming in 2016 from The Penguin Press. A collection of his film writing will be published by Penguin in 2017. " +
                             "\n<br/><br/>\nHe lives in Brooklyn with his family.";
        
        string correctText = "A. O. Scott joined The New York Times as a film critic in January 2000," +
                             " and was named a chief critic in 2004. Previously, " +
                             "Mr. Scott had been the lead Sunday book reviewer for Newsday and a frequent contributor to Slate, " +
                             "The New York Review of Books, and many other publications. " +
                             "\n \n" +
                             "In the 1990s he served on the editorial staffs of Lingua Franca and The New York Review of Books. He also edited" +
                             " \"A Bolt from the Blue and Other Essays,\" " +
                             "a collection by Mary McCarthy, which was published by The New York Review of Books in 2002." +
                             " \n \nMr. Scott was a finalist for the Pulitzer Prize in Criticism in 2010, " +
                             "the same year he served as co-host (with Michael Phillips of the Chicago Tribune) on the last season of" +
                             " \"At the Movies,\" the syndicated film-reviewing program started by Roger Ebert and Gene Siskel." +
                             "\n \nA frequent presence on radio and television, Mr. Scott is Distinguished Professor" +
                             " of Film Criticism at Wesleyan University and the author of Better Living Through Criticism, " +
                             "forthcoming in 2016 from The Penguin Press. A collection of his film writing will be published by Penguin in 2017. " +
                             "\n \nHe lives in Brooklyn with his family.";
        
        // act

        MainWindow window = new MainWindow();
        string outMethod = window.DeleteBr(textForTest);
        // assert
        Assert.Equal(correctText, outMethod);
    }
}