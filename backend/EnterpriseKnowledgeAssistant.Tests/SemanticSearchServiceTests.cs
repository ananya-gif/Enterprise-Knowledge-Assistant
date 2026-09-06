using EnterpriseKnowledgeAssistant.Api.Services;

namespace EnterpriseKnowledgeAssistant.Tests;

public class SemanticSearchServiceTests
{
    [Fact]
    public void CosineSimilarity_ShouldReturnOne_ForIdenticalVectors()
    {
        // Arrange
        var vector = new float[] { 1, 2, 3 };

        // Act
        var similarity =
            SemanticSearchService.CosineSimilarity(vector, vector);

        // Assert
        Assert.Equal(1.0, similarity, precision: 5);
    }

    [Fact]
    public void CosineSimilarity_ShouldReturnZero_ForOrthogonalVectors()
    {
        // Arrange
        var vectorA = new float[] { 1, 0 };
        var vectorB = new float[] { 0, 1 };

        // Act
        var similarity =
            SemanticSearchService.CosineSimilarity(vectorA, vectorB);

        // Assert
        Assert.Equal(0.0, similarity, precision: 5);
    }

    [Fact]
    public void CosineSimilarity_ShouldReturnZero_WhenVectorHasZeroMagnitude()
    {
        // Arrange
        var vectorA = new float[] { 0, 0 };
        var vectorB = new float[] { 1, 2 };

        // Act
        var similarity =
            SemanticSearchService.CosineSimilarity(vectorA, vectorB);

        // Assert
        Assert.Equal(0.0, similarity, precision: 5);
    }

    [Fact]
    public void CosineSimilarity_ShouldThrow_WhenDimensionsDoNotMatch()
    {
        // Arrange
        var vectorA = new float[] { 1, 2 };
        var vectorB = new float[] { 1, 2, 3 };

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            SemanticSearchService.CosineSimilarity(
                vectorA,
                vectorB));
    }
}